using ClosedXML.Excel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data;
using System.Drawing.Text;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;

namespace Certif
{
    public partial class lblHours : Form
    {


        // Переменные для хранения загруженных данных
        private Image backgroundImage;
        private string fontFilePath = "";
        private DataTable dataTable; // <-- Добавили эту строку для хранения таблицы

        // Новые переменные для работы со шрифтами
        private PrivateFontCollection privateFonts = new PrivateFontCollection();
        private Font fontFio;
        private Font fontReg;
        private Font fontHours;
        private Font fontWorkshop; // <-- Добавили для Мастерской
        private Font fontLevel;    // <-- Добавили для Уровня освоения
        private Font fontSquad;

        private string activePlacementMode = null; // Может быть "FIO", "REG" или null
        private const string SKIP_OPTION = "-- Не печатать --";

        public lblHours()
        {
            InitializeComponent();

            // 1. Блокируем поля координаты X для авто-центрирования (они будут серыми)
            numFioX.Enabled = false; numFioX.BackColor = SystemColors.Control;
            numRegX.Enabled = false; numRegX.BackColor = SystemColors.Control;
            numWorkshopX.Enabled = false; numWorkshopX.BackColor = SystemColors.Control; // <-- Новое

            // 2. Привязка событий для ФИО и Региона
            numFioY.ValueChanged += (s, e) => UpdatePreview();
            numFioSize.ValueChanged += (s, e) => { LoadFonts(); };

            numRegY.ValueChanged += (s, e) => UpdatePreview();
            numRegSize.ValueChanged += (s, e) => { LoadFonts(); };

            // 3. Привязка событий для Часов
            txtHours.TextChanged += (s, e) => UpdatePreview();
            numHoursX.ValueChanged += (s, e) => UpdatePreview();
            numHoursY.ValueChanged += (s, e) => UpdatePreview();
            numHoursSize.ValueChanged += (s, e) => { LoadFonts(); };

            // 4. Привязка событий для НОВЫХ полей: Мастерская/Студия
            cmbWorkshopCol.SelectedIndexChanged += (s, e) => UpdatePreview();
            numWorkshopY.ValueChanged += (s, e) => UpdatePreview();
            numWorkshopSize.ValueChanged += (s, e) => { LoadFonts(); };

            // 5. Привязка событий для НОВЫХ полей: Уровень освоения
            cmbLevelCol.SelectedIndexChanged += (s, e) => UpdatePreview();
            numLevelX.ValueChanged += (s, e) => UpdatePreview();
            numLevelY.ValueChanged += (s, e) => UpdatePreview();
            numLevelSize.ValueChanged += (s, e) => { LoadFonts(); };

            // 6. Обновление при смене колонок в основных списках
            cmbFioCol.SelectedIndexChanged += (s, e) => UpdatePreview();
            cmbRegCol.SelectedIndexChanged += (s, e) => UpdatePreview();

            cmbSquadCol.SelectedIndexChanged += (s, e) => UpdatePreview();
            numSquadX.ValueChanged += (s, e) => UpdatePreview();
            numSquadY.ValueChanged += (s, e) => UpdatePreview();
            numSquadSize.ValueChanged += (s, e) => { LoadFonts(); };
        }


        private void gbTextSettings_Enter(object sender, EventArgs e)
        {

        }

        private void lblDataInfo_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnSelectBg_Click(object sender, EventArgs e)
        {
            // Создаем диалог выбора файла
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Разрешаем выбирать только картинки
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Выберите фон сертификата";

                // Если пользователь выбрал файл и нажал "ОК"
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Загружаем картинку в нашу переменную
                        backgroundImage = Image.FromFile(ofd.FileName);

                        // Показываем её в PictureBox
                        pbPreview.Image = backgroundImage;

                        // Обновляем статус
                        lblStatus.Text = "✅ Фон успешно загружен.";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSelectFont_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Разрешаем выбирать только файлы шрифтов
                ofd.Filter = "Файлы шрифтов TrueType|*.ttf";
                ofd.Title = "Выберите файл шрифта (.ttf)";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Сохраняем путь к файлу
                    fontFilePath = ofd.FileName;

                    // Показываем только имя файла (без полного пути), чтобы не загромождать экран
                    lblFontName.Text = Path.GetFileName(fontFilePath);
                    LoadFonts(); // <-- Добавьте эту строку
                    lblStatus.Text = "✅ Шрифт выбран. (Поддерживает кириллицу?)";
                }
            }
        }

        private void btnSelectData_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Таблицы|*.xlsx;*.csv";
                ofd.Title = "Выберите файл со списком награждаемых";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        dataTable = new DataTable();
                        string ext = Path.GetExtension(ofd.FileName).ToLower();

                        // --- ВАРИАНТ 1: Чтение Excel (.xlsx) ---
                        if (ext == ".xlsx")
                        {
                            using (var workbook = new XLWorkbook(ofd.FileName))
                            {
                                var worksheet = workbook.Worksheet(1); // Берем первый лист
                                var range = worksheet.RangeUsed();     // Получаем область с данными

                                if (range == null)
                                {
                                    throw new Exception("Лист Excel пуст.");
                                }

                                // 1. Создаем колонки на основе первой строки (заголовки)
                                foreach (var cell in range.Row(1).Cells())
                                {
                                    dataTable.Columns.Add(cell.GetString().Trim());
                                }

                                // 2. Заполняем строки данными, начиная со второй строки
                                for (int row = 2; row <= range.RowCount(); row++)
                                {
                                    var dataRow = dataTable.NewRow();
                                    int colIndex = 0;

                                    foreach (var cell in range.Row(row).Cells())
                                    {
                                        // Если колонок в строке меньше, чем в заголовке, заполняем пустотой
                                        if (colIndex < dataTable.Columns.Count)
                                        {
                                            dataRow[colIndex] = cell.GetString();
                                        }
                                        colIndex++;
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                        // --- ВАРИАНТ 2: Чтение CSV (.csv) ---
                        else if (ext == ".csv")
                        {
                            // Читаем все строки файла в кодировке UTF-8
                            string[] lines = File.ReadAllLines(ofd.FileName, System.Text.Encoding.UTF8);
                            if (lines.Length > 0)
                            {
                                // Первая строка - это заголовки колонок (разделитель точка с запятой)
                                string[] headers = lines[0].Split(';');
                                foreach (string header in headers)
                                {
                                    dataTable.Columns.Add(header.Trim());
                                }

                                // Остальные строки - это данные
                                for (int i = 1; i < lines.Length; i++)
                                {
                                    if (string.IsNullOrWhiteSpace(lines[i])) continue; // Пропускаем пустые строки
                                    dataTable.Rows.Add(lines[i].Split(';'));
                                }
                            }
                        }

                        // Обновляем интерфейс
                        lblDataInfo.Text = $"✅ Загружено: {dataTable.Rows.Count} строк";
                        lblStatus.Text = "Таблица успешно загружена.";

                        // Очищаем старые значения в выпадающих списках
                        // 1. Очищаем списки и сразу добавляем опцию пропуска
                        cmbFioCol.Items.Clear(); cmbFioCol.Items.Add(SKIP_OPTION);
                        cmbRegCol.Items.Clear(); cmbRegCol.Items.Add(SKIP_OPTION);
                        cmbWorkshopCol.Items.Clear(); cmbWorkshopCol.Items.Add(SKIP_OPTION);
                        cmbLevelCol.Items.Clear(); cmbLevelCol.Items.Add(SKIP_OPTION);
                        cmbSquadCol.Items.Clear(); cmbSquadCol.Items.Add(SKIP_OPTION);

                        // 2. Заполняем списки названиями колонок из таблицы
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            cmbFioCol.Items.Add(col.ColumnName);
                            cmbRegCol.Items.Add(col.ColumnName);
                            cmbWorkshopCol.Items.Add(col.ColumnName);
                            cmbLevelCol.Items.Add(col.ColumnName);
                            cmbSquadCol.Items.Add(col.ColumnName);
                        }

                        // 3. Умный автовыбор колонок
                        foreach (DataColumn col in dataTable.Columns)
                        {
                            string colName = col.ColumnName;
                            string lower = colName.ToLower();

                            if (lower.Contains("фио") || lower.Contains("имя") || lower.Contains("фамилия")) cmbFioCol.SelectedItem = colName;
                            if (lower.Contains("регион") || lower.Contains("город") || lower.Contains("область")) cmbRegCol.SelectedItem = colName;
                            if (lower.Contains("мастерская") || lower.Contains("студии") || lower.Contains("цех") || lower.Contains("лаборатория")) cmbWorkshopCol.SelectedItem = colName;
                            if (lower.Contains("уровень") || lower.Contains("квалификация") || lower.Contains("разряд") || lower.Contains("класс")) cmbLevelCol.SelectedItem = colName;
                            if (lower.Contains("отряд") || lower.Contains("команда") || lower.Contains("группа") || lower.Contains("смена")) cmbSquadCol.SelectedItem = colName;
                        }

                        // 4. Если автовыбор не сработал, принудительно ставим "-- Не печатать --"
                        if (cmbFioCol.SelectedItem == null) cmbFioCol.SelectedItem = SKIP_OPTION;
                        if (cmbRegCol.SelectedItem == null) cmbRegCol.SelectedItem = SKIP_OPTION;
                        if (cmbWorkshopCol.SelectedItem == null) cmbWorkshopCol.SelectedItem = SKIP_OPTION;
                        if (cmbLevelCol.SelectedItem == null) cmbLevelCol.SelectedItem = SKIP_OPTION;
                        if (cmbSquadCol.SelectedItem == null) cmbSquadCol.SelectedItem = SKIP_OPTION;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка чтения файла:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Ошибка при загрузке таблицы.";
                    }
                }
            }
        }

        // Метод для загрузки выбранного шрифта в память программы
        private void LoadFonts()
        {
            if (string.IsNullOrEmpty(fontFilePath) || !File.Exists(fontFilePath)) return;

            try
            {
                privateFonts = new PrivateFontCollection();
                privateFonts.AddFontFile(fontFilePath);

                fontFio = new Font(privateFonts.Families[0], (float)numFioSize.Value, FontStyle.Regular);
                fontReg = new Font(privateFonts.Families[0], (float)numRegSize.Value, FontStyle.Regular);
                fontHours = new Font(privateFonts.Families[0], (float)numHoursSize.Value, FontStyle.Regular); // <-- Новое

                fontWorkshop = new Font(privateFonts.Families[0], (float)numWorkshopSize.Value, FontStyle.Regular);
                fontLevel = new Font(privateFonts.Families[0], (float)numLevelSize.Value, FontStyle.Regular);

                fontSquad = new Font(privateFonts.Families[0], (float)numSquadSize.Value, FontStyle.Regular);

                UpdatePreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки шрифта: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Метод для отрисовки текста на картинке и показа в PictureBox
        private void UpdatePreview()
        {
            if (backgroundImage == null) return;

            try
            {
                Bitmap previewBitmap = new Bitmap(backgroundImage.Width, backgroundImage.Height);

                using (Graphics g = Graphics.FromImage(previewBitmap))
                {
                    g.DrawImage(backgroundImage, 0, 0, backgroundImage.Width, backgroundImage.Height);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                    // 1. ФИО
                    if (fontFio != null && cmbFioCol.SelectedItem?.ToString() != SKIP_OPTION)
                    {
                        string fioText = "Иванов Иван Иванович";
                        if (dataTable != null && dataTable.Rows.Count > 0 && cmbFioCol.SelectedItem != null)
                            fioText = dataTable.Rows[0][cmbFioCol.SelectedItem.ToString()].ToString();

                        SizeF fioSize = g.MeasureString(fioText, fontFio);
                        float fioX = Math.Max(0, (backgroundImage.Width - fioSize.Width) / 2f);
                        if ((int)fioX != numFioX.Value) numFioX.Value = (int)fioX;
                        g.DrawString(fioText, fontFio, Brushes.Black, fioX, (float)numFioY.Value);
                    }

                    // 2. Регион
                    if (fontReg != null && cmbRegCol.SelectedItem?.ToString() != SKIP_OPTION)
                    {
                        string regText = "г. Москва";
                        if (dataTable != null && dataTable.Rows.Count > 0 && cmbRegCol.SelectedItem != null)
                            regText = dataTable.Rows[0][cmbRegCol.SelectedItem.ToString()].ToString();

                        SizeF regSize = g.MeasureString(regText, fontReg);
                        float regX = Math.Max(0, (backgroundImage.Width - regSize.Width) / 2f);
                        if ((int)regX != numRegX.Value) numRegX.Value = (int)regX;
                        g.DrawString(regText, fontReg, Brushes.Black, regX, (float)numRegY.Value);
                    }

                    // 3. Часы (Тут без таблицы, но оставим проверку на всякий случай, если захотите расширить)
                    if (fontHours != null)
                    {
                        string hoursText = string.IsNullOrWhiteSpace(txtHours.Text) ? " " : txtHours.Text;
                        g.DrawString(hoursText, fontHours, Brushes.Black, (float)numHoursX.Value, (float)numHoursY.Value);
                    }

                    // 4. Мастерская
                    if (fontWorkshop != null && cmbWorkshopCol.SelectedItem?.ToString() != SKIP_OPTION)
                    {
                        string workshopText = "Мастерская 'Золотые руки'";
                        if (dataTable != null && dataTable.Rows.Count > 0 && cmbWorkshopCol.SelectedItem != null)
                            workshopText = dataTable.Rows[0][cmbWorkshopCol.SelectedItem.ToString()].ToString();

                        SizeF workshopSize = g.MeasureString(workshopText, fontWorkshop);
                        float workshopX = Math.Max(0, (backgroundImage.Width - workshopSize.Width) / 2f);
                        if ((int)workshopX != numWorkshopX.Value) numWorkshopX.Value = (int)workshopX;
                        g.DrawString(workshopText, fontWorkshop, Brushes.Black, workshopX, (float)numWorkshopY.Value);
                    }

                    // 5. Уровень
                    if (fontLevel != null && cmbLevelCol.SelectedItem?.ToString() != SKIP_OPTION)
                    {
                        string levelText = "Продвинутый";
                        if (dataTable != null && dataTable.Rows.Count > 0 && cmbLevelCol.SelectedItem != null)
                            levelText = dataTable.Rows[0][cmbLevelCol.SelectedItem.ToString()].ToString();

                        g.DrawString(levelText, fontLevel, Brushes.Black, (float)numLevelX.Value, (float)numLevelY.Value);
                    }

                    // 6. Отряд
                    if (fontSquad != null && cmbSquadCol.SelectedItem?.ToString() != SKIP_OPTION)
                    {
                        string squadText = "Отряд 'Лидер'";
                        if (dataTable != null && dataTable.Rows.Count > 0 && cmbSquadCol.SelectedItem != null)
                            squadText = dataTable.Rows[0][cmbSquadCol.SelectedItem.ToString()].ToString();

                        g.DrawString(squadText, fontSquad, Brushes.Black, (float)numSquadX.Value, (float)numSquadY.Value);
                    }
                }

                if (pbPreview.Image != null && pbPreview.Image != backgroundImage)
                {
                    pbPreview.Image.Dispose();
                }
                pbPreview.Image = previewBitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка предпросмотра: " + ex.Message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // 1. Проверки: всё ли готово к генерации?
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала выберите фоновое изображение!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Сначала загрузите таблицу с данными!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (fontFio == null || fontReg == null)
            {
                MessageBox.Show("Сначала выберите файл шрифта (.ttf)!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbFioCol.SelectedItem == null || cmbRegCol.SelectedItem == null)
            {
                MessageBox.Show("Выберите колонки для ФИО и Региона в выпадающих списках!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Выбор места сохранения файла
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PDF файлы|*.pdf";
                sfd.FileName = "Сертификаты.pdf";
                sfd.Title = "Сохранить сертификаты как...";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    lblStatus.Text = "⏳ Генерация... Пожалуйста, подождите.";
                    Application.DoEvents(); // Обновляем интерфейс, чтобы текст статуса успел измениться

                    try
                    {
                        string fioCol = cmbFioCol.SelectedItem.ToString();
                        string regCol = cmbRegCol.SelectedItem.ToString();
                        int totalRows = dataTable.Rows.Count;

                        // 3. Создаем новый PDF документ
                        using (PdfDocument pdf = new PdfDocument())
                        {
                            for (int i = 0; i < totalRows; i++)
                            {
                                DataRow row = dataTable.Rows[i];

                                lblStatus.Text = $"⏳ Генерация: {i + 1} из {totalRows}...";
                                Application.DoEvents();

                                // 4. Создаем чистый холст
                                using (Bitmap bmp = new Bitmap(backgroundImage.Width, backgroundImage.Height))
                                {
                                    using (Graphics g = Graphics.FromImage(bmp))
                                    {
                                        // !!! ИСПРАВЛЕНИЕ 1: Заливаем холст белым цветом. 
                                        // Это предотвращает превращение прозрачных областей в черные при сохранении в JPEG.
                                        g.Clear(Color.White);

                                        // Рисуем исходный фон поверх белого
                                        g.DrawImage(backgroundImage, 0, 0, backgroundImage.Width, backgroundImage.Height);
                                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                                        // --- ОТРИСОВКА ФИО ---
                                        if (cmbFioCol.SelectedItem?.ToString() != SKIP_OPTION)
                                        {
                                            string fioText = row[cmbFioCol.SelectedItem.ToString()].ToString();
                                            SizeF fioSize = g.MeasureString(fioText, fontFio);
                                            float fioX = Math.Max(0, (backgroundImage.Width - fioSize.Width) / 2f);
                                            g.DrawString(fioText, fontFio, Brushes.Black, fioX, (float)numFioY.Value);
                                        }

                                        // --- ОТРИСОВКА РЕГИОНА ---
                                        if (cmbRegCol.SelectedItem?.ToString() != SKIP_OPTION)
                                        {
                                            string regText = row[cmbRegCol.SelectedItem.ToString()].ToString();
                                            SizeF regSize = g.MeasureString(regText, fontReg);
                                            float regX = Math.Max(0, (backgroundImage.Width - regSize.Width) / 2f);
                                            g.DrawString(regText, fontReg, Brushes.Black, regX, (float)numRegY.Value);
                                        }

                                        // --- ОТРИСОВКА ЧАСОВ ---
                                        string hoursText = txtHours.Text;
                                        if (string.IsNullOrWhiteSpace(hoursText)) hoursText = " ";
                                        g.DrawString(hoursText, fontHours, Brushes.Black, (float)numHoursX.Value, (float)numHoursY.Value);

                                        // --- ОТРИСОВКА МАСТЕРСКОЙ ---
                                        if (cmbWorkshopCol.SelectedItem?.ToString() != SKIP_OPTION)
                                        {
                                            string workshopText = row[cmbWorkshopCol.SelectedItem.ToString()].ToString();
                                            SizeF workshopSize = g.MeasureString(workshopText, fontWorkshop);
                                            float workshopX = Math.Max(0, (backgroundImage.Width - workshopSize.Width) / 2f);
                                            g.DrawString(workshopText, fontWorkshop, Brushes.Black, workshopX, (float)numWorkshopY.Value);
                                        }

                                        // --- ОТРИСОВКА УРОВНЯ ---
                                        if (cmbLevelCol.SelectedItem?.ToString() != SKIP_OPTION)
                                        {
                                            string levelText = row[cmbLevelCol.SelectedItem.ToString()].ToString();
                                            g.DrawString(levelText, fontLevel, Brushes.Black, (float)numLevelX.Value, (float)numLevelY.Value);
                                        }

                                        // --- ОТРИСОВКА ОТРЯДА ---
                                        if (cmbSquadCol.SelectedItem?.ToString() != SKIP_OPTION)
                                        {
                                            string squadText = row[cmbSquadCol.SelectedItem.ToString()].ToString();
                                            g.DrawString(squadText, fontSquad, Brushes.Black, (float)numSquadX.Value, (float)numSquadY.Value);
                                        }
                                    }

                                    // 5. Конвертируем Bitmap в поток (MemoryStream) для PdfSharp
                                    using (MemoryStream ms = new MemoryStream())
                                    {
                                        var encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                                        encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 95L);
                                        var jpegCodec = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == System.Drawing.Imaging.ImageFormat.Jpeg.Guid);

                                        bmp.Save(ms, jpegCodec, encoderParams);

                                        // !!! ИСПРАВЛЕНИЕ 2: Сбрасываем позицию потока в самое начало перед чтением !!!
                                        ms.Position = 0;

                                        // Создаем изображение для PDF из потока
                                        XImage xImg = XImage.FromStream(ms);

                                        // 6. Добавляем новую страницу в PDF
                                        PdfPage page = pdf.AddPage();
                                        page.Width = XUnit.FromPoint(bmp.Width);
                                        page.Height = XUnit.FromPoint(bmp.Height);

                                        // Рисуем изображение на странице PDF
                                        using (XGraphics gfx = XGraphics.FromPdfPage(page))
                                        {
                                            gfx.DrawImage(xImg, 0, 0, page.Width, page.Height);
                                        }
                                    }
                                }
                            }

                            // 7. Сохраняем итоговый PDF файл на диск
                            pdf.Save(sfd.FileName);
                        }

                        lblStatus.Text = $"✅ Готово! Создано {totalRows} сертификатов.";
                        MessageBox.Show($"Успешно создано {totalRows} сертификатов!\nФайл сохранен:\n{sfd.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при генерации PDF:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Произошла ошибка при генерации.";
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "FIO";
            pbPreview.Cursor = Cursors.Cross; // Меняем курсор на прицел
            lblStatus.Text = "🎯 Кликните по изображению, чтобы разместить ФИО";
        }

        private void btnPlaceReg_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "REG";
            pbPreview.Cursor = Cursors.Cross; // Меняем курсор на прицел
            lblStatus.Text = "🎯 Кликните по изображению, чтобы разместить Регион";
        }


        private void pbPreview_MouseClick(object sender, MouseEventArgs e)
        {
            if (activePlacementMode == null || backgroundImage == null) return;

            Point imgPoint = GetImageCoordinatesFromMouse(e.Location);

            if (activePlacementMode == "FIO")
            {
                numFioY.Value = imgPoint.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "REG")
            {
                numRegY.Value = imgPoint.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "HOURS")
            {
                numHoursX.Value = imgPoint.X; // <-- ДОБАВИЛИ: обновляем и X
                numHoursY.Value = imgPoint.Y; // Обновляем Y
                activePlacementMode = null;
            }
            else if (activePlacementMode == "WORKSHOP")
            {
                numWorkshopY.Value = imgPoint.Y; // Только Y, так как X авто-центрируется
                activePlacementMode = null;
            }
            else if (activePlacementMode == "LEVEL")
            {
                numLevelX.Value = imgPoint.X; // И X
                numLevelY.Value = imgPoint.Y; // И Y
                activePlacementMode = null;
            }
            else if (activePlacementMode == "SQUAD")
            {
                numSquadX.Value = imgPoint.X;
                numSquadY.Value = imgPoint.Y;
                activePlacementMode = null;
            }

            pbPreview.Cursor = Cursors.Default;
            lblStatus.Text = "✅ Высота обновлена. Текст автоматически центрирован по горизонтали.";
            UpdatePreview();
        }

        private Point GetImageCoordinatesFromMouse(Point mousePoint)
        {
            float imgRatio = (float)backgroundImage.Width / backgroundImage.Height;
            float boxRatio = (float)pbPreview.ClientSize.Width / pbPreview.ClientSize.Height;

            float scale;
            int offsetX = 0, offsetY = 0;

            // Определяем, как именно картинка вписана в PictureBox (по ширине или по высоте)
            if (imgRatio > boxRatio)
            {
                // Картинка упирается в боковые края
                scale = (float)pbPreview.ClientSize.Width / backgroundImage.Width;
                offsetY = (int)((pbPreview.ClientSize.Height - (backgroundImage.Height * scale)) / 2);
            }
            else
            {
                // Картинка упирается в верх и низ
                scale = (float)pbPreview.ClientSize.Height / backgroundImage.Height;
                offsetX = (int)((pbPreview.ClientSize.Width - (backgroundImage.Width * scale)) / 2);
            }

            // Переводим координаты мыши в координаты картинки
            int imgX = (int)((mousePoint.X - offsetX) / scale);
            int imgY = (int)((mousePoint.Y - offsetY) / scale);

            // Защита от выхода за границы (на всякий случай)
            imgX = Math.Max(0, Math.Min(imgX, backgroundImage.Width));
            imgY = Math.Max(0, Math.Min(imgY, backgroundImage.Height));

            return new Point(imgX, imgY);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && activePlacementMode != null)
            {
                activePlacementMode = null;
                pbPreview.Cursor = Cursors.Default;
                lblStatus.Text = "⛔ Размещение отменено.";
            }
        }

        private void btnPlaceHours_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "HOURS";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы задать высоту для 'Количества часов'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPlaceWorkshop_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "WORKSHOP";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы задать высоту для Мастерской";
        }

        private void btnPlaceLevel_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "LEVEL";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы разместить Уровень освоения";
        }

        private void btnPlaceSquad_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "SQUAD";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы разместить Отряд";
        }       
    }


}


