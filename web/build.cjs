// Rebuild the single-file offline application. No dependency installation required.
const fs = require('node:fs');
const path = require('node:path');
const root = __dirname;
const read = file => fs.readFileSync(path.join(root,file),'utf8').replace(/^\uFEFF/,'');
const script = text => text.replace(/<\/script/gi,'<\\/script');
const presets = {};
for(const file of fs.readdirSync(path.join(root,'assets/presets')).filter(file=>file.endsWith('.json')).sort()){
 const preset=JSON.parse(read('assets/presets/'+file));
 if(typeof preset.PresetName!=='string')throw new Error('Missing PresetName: '+file);
 presets['preset:'+file]=preset;
}
const assets = 'const BUILTIN_PRESETS='+JSON.stringify(presets)+';\nconst BUILTIN_FONT_NAME="RobotoSerif-VariableFont_GRAD,opsz,wdth,wght.ttf";\nconst BUILTIN_FONT='+JSON.stringify(fs.readFileSync(path.join(root,'assets/RobotoSerif.ttf')).toString('base64'))+';';
let html=read('src/index.html');
for(const [marker,content] of [['STYLES',read('src/style.css')],['XLSX',script(read('vendor/xlsx.full.min.js'))],['PDFLIB',script(read('vendor/pdf-lib.min.js'))],['ASSETS',script(assets)],['APP',script(read('src/app.js'))]])html=html.replace('/* '+marker+' */',()=>content);
const notices=['assets/RobotoSerif.OFL.txt','vendor/xlsx.LICENSE.txt','vendor/pdf-lib.LICENSE.md'].map(file=>file+'\n'+read(file)).join('\n\n');
html=html.replace(/<\/body>\s*<\/html>\s*$/,'<script type='+JSON.stringify('text/plain')+' id='+JSON.stringify('third-party-licenses')+'>'+script(notices)+'</script></body></html>');
fs.writeFileSync(path.join(root,'Сертификаты.html'),html,'utf8');
console.log('Built Сертификаты.html: '+(Buffer.byteLength(html)/1024/1024).toFixed(2)+' MB');


