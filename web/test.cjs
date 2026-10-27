const {chromium}=require('playwright');
const {PDFDocument}=require('pdf-lib');
const fs=require('node:fs');const path=require('node:path');const assert=require('node:assert/strict');const {pathToFileURL}=require('node:url');
async function downloadBytes(page,action){const waiting=page.waitForEvent('download');await action();const download=await waiting;const file=await download.path();return fs.readFileSync(file);}
(async()=>{
 const browser=await chromium.launch({executablePath:process.env.BROWSER_EXECUTABLE,headless:true});
 try{
 const context=await browser.newContext({viewport:{width:1500,height:1000},acceptDownloads:true});
 const page=await context.newPage();const errors=[];page.on('pageerror',e=>errors.push(e.message));page.on('dialog',d=>d.accept());
 await context.setOffline(true);
 await page.goto(pathToFileURL(path.join(__dirname,'Сертификаты.html')).href);await page.waitForFunction(()=>document.getElementById('fields').children.length===8);
 assert.equal(await page.evaluate(()=>document.fonts.check('20px "Roboto Serif"')),true);
 // A field can inherit the shared font or override it, with independent styles.
 await page.selectOption('#font','Arial');
 await page.check('#ownFont');await page.selectOption('#fieldFont','Georgia');
 await page.click('#bold');await page.click('#italic');
 await page.getByRole('button',{name:'Основной текст',exact:true}).click();
 assert.equal(await page.locator('#ownFont').isChecked(),false);
 assert.equal(await page.locator('#fieldFont').isDisabled(),true);
 assert.equal(await page.locator('#bold').getAttribute('aria-pressed'),'false');
 await page.selectOption('#font','Verdana');
 assert.equal(await page.locator('#fieldFont').inputValue(),'Verdana');
 await page.getByRole('button',{name:'ФИО',exact:true}).click();
 assert.equal(await page.locator('#fieldFont').inputValue(),'Georgia');
 assert.equal(await page.locator('#bold').getAttribute('aria-pressed'),'true');
 assert.equal(await page.locator('#italic').getAttribute('aria-pressed'),'true');
 // Inspect actual canvas text drawing for preview and PDF rendering.
 const drawn=await page.evaluate(()=>{
  const fonts=[],original=CanvasRenderingContext2D.prototype.fillText;
  CanvasRenderingContext2D.prototype.fillText=function(text,...args){fonts.push({text,font:this.font});return original.call(this,text,...args);};
  try{paint(document.createElement('canvas'),0,true);}finally{CanvasRenderingContext2D.prototype.fillText=original;}
  return fonts;
 });
 assert.match(drawn.find(f=>f.text==='Имя Фамилия').font,/italic.*bold.*Georgia/);
 assert.match(drawn.find(f=>f.text.includes('за участие')).font,/Verdana/);
 const stylePreset=await downloadBytes(page,()=>page.click('#savePreset'));
 await page.selectOption('#preset','default');
 await page.setInputFiles('#presetFile',{name:'styles.json',mimeType:'application/json',buffer:stylePreset});
 await page.waitForFunction(()=>state.fields.fio.ownFont&&state.fields.fio.bold&&state.fields.fio.italic);
 assert.equal(await page.locator('#fieldFont').inputValue(),'Georgia');
 await page.uncheck('#ownFont');assert.equal(await page.locator('#fieldFont').inputValue(),'Verdana');
 const legacy=JSON.parse(stylePreset);for(const f of Object.values(legacy.fields)){delete f.font;delete f.ownFont;delete f.bold;delete f.italic;}
 await page.setInputFiles('#presetFile',{name:'old.json',mimeType:'application/json',buffer:Buffer.from(JSON.stringify(legacy))});
 await page.waitForFunction(()=>!state.fields.fio.bold);
 assert.equal(await page.locator('#fieldFont').inputValue(),'Verdana');
 assert.equal(await page.locator('#ownFont').isChecked(),false);
 await page.selectOption('#preset','default');
 await page.click('#export');assert.match(await page.locator('#status').textContent(),/Добавьте список/);
 await page.click('#manual');await page.fill('#names','Иванова Анна Сергеевна\nПетров Михаил Андреевич');await page.click('#useNames');assert.match(await page.locator('#dataName').textContent(),/2 строк/);
 await page.getByRole('button',{name:'Основной текст',exact:true}).click();await page.fill('#text','За успехи в учёбе\nи стремление к знаниям');
 assert.equal(await page.evaluate(()=>textFor(state.fields.main,0)),'За успехи в учёбе\nи стремление к знаниям');
 await page.click('#next');assert.match(await page.locator('#person option:checked').textContent(),/Петров/);
 await page.selectOption('#paper','portrait');const pdfBytes=await downloadBytes(page,()=>page.click('#export'));const pdf=await PDFDocument.load(pdfBytes);assert.equal(pdf.getPageCount(),2);assert.ok(Math.abs(pdf.getPage(0).getWidth()-595.276)<.1);
 await page.selectOption('#scope','current');const currentPdf=await PDFDocument.load(await downloadBytes(page,()=>page.click('#export')));assert.equal(currentPdf.getPageCount(),1);
 await page.getByRole('button',{name:'ФИО',exact:true}).click();const before=Number(await page.locator('#y').inputValue());await page.locator('#preview').focus();await page.keyboard.press('Shift+ArrowDown');assert.equal(Number(await page.locator('#y').inputValue()),before+10);
 // Automatic header detection and existing letter-based field mappings.
 await page.evaluate(()=>{
   if(detectHeaderRow([['ФИО','Регион'],['Анна','Москва']])!==1)throw Error('Ordinary headers skipped');
   if(detectHeaderRow([['Отчёт',''],['Имя','Город']], [{s:{r:0,c:0},e:{r:0,c:1}}])!==2)throw Error('Merge not recognized');
 });
 await page.fill('#headerRow','');
 await page.setInputFiles('#dataFile',{name:'auto.csv',mimeType:'text/csv',buffer:Buffer.from('Протокол;;\nФИО;Регион;Отряд\nАнна;Москва;1')});
 await page.waitForFunction(()=>rows[0]?.[0]==='Анна');
 assert.equal(await page.evaluate(()=>rows.length),1);
 assert.match(await page.locator('#dataName').textContent(),/Заголовки: строка 2/);
 // A document title and a blank row must not shift the selected physical header row.
 await page.fill('#headerRow','3');
 await page.setInputFiles('#dataFile',{name:'title.csv',mimeType:'text/csv',buffer:Buffer.from('Протокол;;\n;;\nФИО;Регион;\nАнна;Москва;')});
 await page.waitForFunction(()=>rows[0]?.[0]==='Анна');
 assert.equal(await page.evaluate(()=>rows.length),1);
 await page.getByRole('button',{name:'ФИО',exact:true}).click();
 assert.match(await page.locator('#source').textContent(),/B — Регион/);
 await page.fill('#headerRow','1');
 const csv='ФИО;Текст;Регион\r\n"Иванов, Иван";"Первая строка\nВторая; строка с ""кавычками""";Владивосток\r\nПетров;Спасибо;Хабаровск';
 await page.setInputFiles('#dataFile',{name:'participants.csv',mimeType:'text/csv',buffer:Buffer.from(csv)});await page.waitForFunction(()=>rows[0]?.[0]==='Иванов, Иван');assert.equal(await page.evaluate(()=>rows[0][1]),'Первая строка\nВторая; строка с "кавычками"');
 await page.getByRole('button',{name:'Основной текст',exact:true}).click();await page.selectOption('#source','Текст');
 const presetBytes=await downloadBytes(page,()=>page.click('#savePreset'));const saved=JSON.parse(presetBytes);assert.equal(saved.fields.main.source,'Текст');assert.equal(saved.rows,undefined);
 await page.selectOption('#preset','preset:Диплом ДИТЦ.json');await page.waitForFunction(()=>state.width===2480);assert.equal(await page.locator('#font').inputValue(),'Roboto Serif');assert.equal(await page.evaluate(()=>state.fields.fio.y),1515);assert.equal(await page.evaluate(()=>state.fields.main.source),null);
 await page.setInputFiles('#presetFile',{name:'saved.json',mimeType:'application/json',buffer:presetBytes});await page.waitForFunction(()=>state.width===1240);assert.equal(await page.evaluate(()=>state.fields.main.source),'Текст');
 await page.setInputFiles('#presetFile',path.join(__dirname,'assets','presets','Диплом ДИТЦ.json'));await page.waitForFunction(()=>state.width===2480);assert.equal(await page.evaluate(()=>state.fields.squad.x),2234);
 await page.selectOption('#preset','default');await page.waitForFunction(()=>state.width===1240);
 const png=await page.evaluate(()=>{const c=document.createElement('canvas');c.width=1000;c.height=700;const g=c.getContext('2d');g.fillStyle='#aabbcc';g.fillRect(0,0,1000,700);return c.toDataURL();});
 await page.setInputFiles('#background',{name:'background.png',mimeType:'image/png',buffer:Buffer.from(png.split(',')[1],'base64')});await page.waitForFunction(()=>state.width===1000);
 const bgSaved=JSON.parse(await downloadBytes(page,()=>page.click('#savePreset')));assert.equal(bgSaved.background,png);assert.equal(bgSaved.height,700);
 await page.setInputFiles('#fontFile',path.join(__dirname,'assets','RobotoSerif.ttf'));await page.waitForFunction(()=>state.font==='UserFont');const fontSaved=await downloadBytes(page,()=>page.click('#savePreset'));assert.match(JSON.parse(fontSaved).fontData,/^data:/);
 await page.selectOption('#preset','preset:Диплом ДИТЦ.json');await page.waitForFunction(()=>state.font==='Roboto Serif');assert.equal(await page.evaluate(()=>state.width),1000);assert.equal(await page.evaluate(()=>state.height),700);assert.equal(await page.evaluate(()=>state.background),png);assert.equal(await page.locator('#backgroundName').textContent(),'background.png');assert.ok(Math.abs(await page.evaluate(()=>state.fields.fio.y)-1515*700/3508)<.001);
 await page.setInputFiles('#presetFile',path.join(__dirname,'assets','presets','Диплом ДИТЦ.json'));await page.waitForFunction(()=>document.getElementById('preset').value==='custom');assert.equal(await page.evaluate(()=>state.background),png);assert.equal(await page.evaluate(()=>state.width),1000);
 await page.setInputFiles('#presetFile',{name:'font.json',mimeType:'application/json',buffer:fontSaved});await page.waitForFunction(()=>state.width===1000&&state.font==='UserFont');assert.equal(await page.evaluate(()=>document.fonts.check('20px UserFont')),true);
 // Keep an embedded custom font used only by one field while the rest use Arial.
 await page.getByRole('button',{name:'ФИО',exact:true}).click();await page.check('#ownFont');
 await page.click('#bold');await page.click('#italic');await page.selectOption('#font','Arial');
 const customOnly=await downloadBytes(page,()=>page.click('#savePreset'));
 assert.equal(JSON.parse(customOnly).font,'Arial');assert.match(JSON.parse(customOnly).fontData,/^data:/);
 await page.selectOption('#preset','default');
 await page.setInputFiles('#presetFile',{name:'custom-only.json',mimeType:'application/json',buffer:customOnly});
 await page.waitForFunction(()=>state.fields.fio.ownFont&&state.fields.fio.font==='UserFont');
 assert.equal(await page.locator('#font').inputValue(),'Arial');assert.equal(await page.locator('#fieldFont').inputValue(),'UserFont');
 const exportFonts=await page.evaluate(()=>{
  const fonts=[],original=CanvasRenderingContext2D.prototype.fillText;
  CanvasRenderingContext2D.prototype.fillText=function(text,...args){fonts.push({text,font:this.font});return original.call(this,text,...args);};
  try{paint(document.createElement('canvas'),0);}finally{CanvasRenderingContext2D.prototype.fillText=original;}
  return fonts;
 });
 assert.match(exportFonts.find(f=>f.text==='Иванов, Иван').font,/italic.*bold.*UserFont/);

 await page.click('#pipette');await page.locator('#preview').click({position:{x:2,y:2}});assert.equal(await page.locator('#color').inputValue(),'#aabbcc');
 // Excel workbook with multiple sheets, Cyrillic, and displayed number/date formatting.
 const xlsx=await page.evaluate(()=>{const w=XLSX.utils.book_new();XLSX.utils.book_append_sheet(w,XLSX.utils.aoa_to_sheet([]),'Пустой');XLSX.utils.book_append_sheet(w,XLSX.utils.aoa_to_sheet([['ФИО','Дата'],['Сидорова','13.10.2026']]),'Первый');XLSX.utils.book_append_sheet(w,XLSX.utils.aoa_to_sheet([['ФИО'],['Второй участник']]),'Второй');return Array.from(new Uint8Array(XLSX.write(w,{bookType:'xlsx',type:'array'})));});
 await page.setInputFiles('#dataFile',{name:'people.xlsx',mimeType:'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',buffer:Buffer.from(xlsx)});await page.waitForFunction(()=>rows[0]?.[0]==='Сидорова');await page.selectOption('#sheet','Второй');assert.equal(await page.evaluate(()=>rows[0][0]),'Второй участник');
 // Drag a selected field, reject clipped output, and cancel a batch.
 await page.getByRole('button',{name:'ФИО',exact:true}).click();
 await page.locator('#preview').scrollIntoViewIfNeeded();
 const box=await page.locator('#preview').boundingBox();
 const pos=await page.evaluate(()=>({x:state.fields.fio.x+30,y:state.fields.fio.y+20,w:state.width,h:state.height}));
 const oldX=Number(await page.locator('#x').inputValue());
 await page.mouse.move(box.x+pos.x*box.width/pos.w,box.y+pos.y*box.height/pos.h);await page.mouse.down();await page.mouse.move(box.x+(pos.x+20)*box.width/pos.w,box.y+(pos.y+20)*box.height/pos.h);await page.mouse.up();
 assert.ok(Number(await page.locator('#x').inputValue())>oldX);
 await page.fill('#y',String(await page.evaluate(()=>state.height-1))); await page.click('#export');await page.waitForFunction(()=>!document.getElementById('progressDialog').open);assert.match(await page.locator('#status').textContent(),/выходит за край/);
 await page.fill('#y','260');await page.click('#manual');await page.fill('#names',Array.from({length:30},(_,i)=>'Участник '+i).join('\n'));await page.click('#useNames');await page.selectOption('#scope','all');await page.click('#export');await page.click('#cancelExport');await page.waitForFunction(()=>!document.getElementById('progressDialog').open);assert.match(await page.locator('#status').textContent(),/отменён/);
 // Google request URL and protocol tested with a controlled response; live smoke is separate.
 await context.setOffline(false);let requested;
 await page.route('https://docs.google.com/**',route=>{requested=new URL(route.request().url());const callback=requested.searchParams.get('tqx').match(/responseHandler:([^;]+)/)[1];return route.fulfill({contentType:'application/javascript',body:callback+'('+JSON.stringify({status:'ok',table:{cols:[{label:'ФИО'},{label:'Отряд'}],rows:[{c:[{v:'Из Google'},{v:7,f:'07'}]}]}})+');'});});
 await page.fill('#headerRow','2');
 await page.fill('#googleUrl','https://docs.google.com/spreadsheets/d/test-sheet/edit#gid=123');await page.click('#loadGoogle');await page.waitForFunction(()=>rows[0]?.[0]==='Из Google');assert.equal(requested.searchParams.get('gid'),'123');assert.equal(requested.searchParams.get('range'),'A2:KN');await page.fill('#headerRow','1');assert.equal(await page.evaluate(()=>rows[0][1]),'07');
 await page.unroute('https://docs.google.com/**');
 await page.route('https://docs.google.com/**',route=>{const u=new URL(route.request().url());const cb=u.searchParams.get('tqx').match(/responseHandler:([^;]+)/)[1];return route.fulfill({contentType:'application/javascript',body:cb+'('+JSON.stringify({status:'ok',table:{cols:[{label:'Протокол',id:'A'},{label:'',id:'B'},{label:'УТВЕРЖДАЮ',id:'C'}],rows:[{c:[{v:'№'},{v:'ФИО'},{v:'Регион'}]},{c:[{v:'1'},{v:'Из Google'},{v:'Москва'}]}]}})+');'});});
 await page.fill('#headerRow','');await page.evaluate(()=>state.fields.fio.source='B');
 await page.click('#loadGoogle');await page.waitForFunction(()=>headers[1]==='ФИО');
 assert.equal(await page.evaluate(()=>rows.length),1);
 assert.equal(await page.evaluate(()=>textFor(state.fields.fio,0)),'Из Google');
 assert.match(await page.locator('#dataName').textContent(),/Заголовки: строка 2/);
 await page.fill('#headerRow','1');
 await page.unroute('https://docs.google.com/**');await page.route('https://docs.google.com/**',route=>route.abort());await page.click('#loadGoogle');await page.waitForFunction(()=>!document.getElementById('loadGoogle').disabled);assert.match(await page.locator('#status').textContent(),/Не удалось загрузить/);assert.equal(await page.evaluate(()=>rows[0][1]),'Из Google');
 await page.fill('#googleUrl','https://evil.example/spreadsheets/d/test');await page.click('#loadGoogle');assert.match(await page.locator('#status').textContent(),/Нужна ссылка/);
 await page.selectOption('#preset','default');assert.equal(await page.evaluate(()=>state.background),png);assert.equal(await page.evaluate(()=>state.width),1000);await page.click('#resetBackground');await page.waitForFunction(()=>state.width===1240);assert.equal(await page.evaluate(()=>state.background),null);await page.setViewportSize({width:1500,height:1000});await page.screenshot({path:path.join(__dirname,'preview.png'),fullPage:true});

 // Height arrows follow visual direction; blank names never produce PDF pages.
 await page.getByRole('button',{name:'ФИО',exact:true}).click();
 const heightBefore=Number(await page.locator('#y').inputValue());
 await page.click('#heightDown');assert.equal(Number(await page.locator('#y').inputValue()),heightBefore+5);
 await page.click('#heightUp');assert.equal(Number(await page.locator('#y').inputValue()),heightBefore);
 await page.locator('#y').press('ArrowDown');assert.equal(Number(await page.locator('#y').inputValue()),heightBefore+5);
 await page.locator('#y').press('ArrowUp');assert.equal(Number(await page.locator('#y').inputValue()),heightBefore);
 const emptyNames='Регион;ФИО\nМосква;\nОмск;   \nКазань;Анна';
 await page.setInputFiles('#dataFile',{name:'empty-names.csv',mimeType:'text/csv',buffer:Buffer.from(emptyNames)});
 await page.waitForFunction(()=>rows.length===3);await page.selectOption('#scope','all');
 const filtered=await PDFDocument.load(await downloadBytes(page,()=>page.click('#export')));assert.equal(filtered.getPageCount(),1);
 assert.match(await page.locator('#status').textContent(),/Пропущено строк без ФИО: 2/);
 let unexpectedDownloads=0;const onDownload=()=>unexpectedDownloads++;page.on('download',onDownload);
 await page.selectOption('#person','0');await page.selectOption('#scope','current');await page.click('#export');assert.match(await page.locator('#status').textContent(),/PDF не создан/);
 await page.setInputFiles('#dataFile',{name:'all-empty.csv',mimeType:'text/csv',buffer:Buffer.from('ФИО;Регион\n;Москва\n   ;Омск')});
 await page.waitForFunction(()=>rows.length===2);await page.selectOption('#scope','all');await page.click('#export');assert.match(await page.locator('#status').textContent(),/PDF не создан/);assert.equal(unexpectedDownloads,0);page.off('download',onDownload);
 await page.setViewportSize({width:390,height:844});assert.equal(await page.evaluate(()=>document.documentElement.scrollWidth<=window.innerWidth),true);
 assert.deepEqual(errors,[]);console.log('PASS: offline file launch; fonts; CSV; XLSX sheets; manual input; PDF pages and A4; keyboard placement; legacy and new presets; embedded background/font roundtrip; pipette; Google protocol/gid/error; responsive layout.');
 }finally{await browser.close();}
})().catch(e=>{console.error(e);process.exit(1);});

