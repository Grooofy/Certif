using ClosedXML.Excel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Certif
{
    public partial class lblHours : Form
    {
        // Переменные для хранения загруженных данных
        private Image backgroundImage;
        private string fontFilePath = "";
        private string selectedSystemFontName = "Arial"; // <-- Новая переменная для системного шрифта
        private DataTable dataTable; // <-- Добавили эту строку для хранения таблицы

        // Новые переменные для работы со шрифтами
        private PrivateFontCollection privateFonts = new PrivateFontCollection();
        private Font fontFio;
        private Font fontReg;
        private Font fontHours;
        private Font fontWorkshop; // <-- Добавили для Мастерской
        private Font fontLevel;    // <-- Добавили для Уровня освоения
        private Font fontSquad;
        private Font fontDate;
        private Font fontMainText;

        private string activePlacementMode = null; // Может быть "FIO", "REG" или null
        private const string SKIP_OPTION = "-- Не печатать --";

        private Color globalTextColor = Color.Black;
        private bool isPipetteMode = false;

        public lblHours()
        {
            InitializeComponent();

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

            // Привязка событий для Даты
            txtDate.TextChanged += (s, e) => UpdatePreview();
            numDateX.ValueChanged += (s, e) => UpdatePreview();
            numDateY.ValueChanged += (s, e) => UpdatePreview();
            numDateSize.ValueChanged += (s, e) => { LoadFonts(); };

            // Привязка событий для Основного текста
            txtMainText.TextChanged += (s, e) => UpdatePreview();
            numMainTextY.ValueChanged += (s, e) => UpdatePreview();
            numMainTextSize.ValueChanged += (s, e) => { LoadFonts(); };
            numMainTextWidth.ValueChanged += (s, e) => UpdatePreview();

            pnlGlobalColor.BackColor = globalTextColor;
            LoadSystemFonts();
        }

        private void LoadSystemFonts()
        {
            cmbFontSelector.Items.Clear();
            cmbFontSelector.Items.Add("📂 Загрузить свой файл шрифта (.ttf)...");

            InstalledFontCollection installedFonts = new InstalledFontCollection();
            List<string> fontNames = new List<string>();

            foreach (FontFamily family in installedFonts.Families)
            {
                // Берем только шрифты, поддерживающие обычный стиль (Regular)
                if (family.IsStyleAvailable(FontStyle.Regular))
                {
                    fontNames.Add(family.Name);
                }
            }

            // Сортируем по алфавиту для удобства
            fontNames.Sort();

            foreach (string name in fontNames)
            {
                cmbFontSelector.Items.Add(name);
            }

            // Пытаемся выбрать Arial по умолчанию, иначе берем первый доступный
            if (fontNames.Contains("Arial"))
            {
                cmbFontSelector.SelectedItem = "Arial";
            }
            else if (cmbFontSelector.Items.Count > 1)
            {
                cmbFontSelector.SelectedIndex = 1; // Пропускаем пункт "Загрузить файл"
            }
        }

        // Метод для обновления выпадающих списков после загрузки любых данных
        private void UpdateUIFromDataTable()
        {
            lblDataInfo.Text = $"✅ Загружено: {dataTable.Rows.Count} строк";
            lblStatus.Text = "Таблица успешно загружена.";

            cmbFioCol.Items.Clear(); cmbFioCol.Items.Add(SKIP_OPTION);
            cmbRegCol.Items.Clear(); cmbRegCol.Items.Add(SKIP_OPTION);
            cmbWorkshopCol.Items.Clear(); cmbWorkshopCol.Items.Add(SKIP_OPTION);
            cmbLevelCol.Items.Clear(); cmbLevelCol.Items.Add(SKIP_OPTION);
            cmbSquadCol.Items.Clear(); cmbSquadCol.Items.Add(SKIP_OPTION);

            foreach (DataColumn col in dataTable.Columns)
            {
                cmbFioCol.Items.Add(col.ColumnName);
                cmbRegCol.Items.Add(col.ColumnName);
                cmbWorkshopCol.Items.Add(col.ColumnName);
                cmbLevelCol.Items.Add(col.ColumnName);
                cmbSquadCol.Items.Add(col.ColumnName);
            }

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

            if (cmbFioCol.SelectedItem == null) cmbFioCol.SelectedItem = SKIP_OPTION;
            if (cmbRegCol.SelectedItem == null) cmbRegCol.SelectedItem = SKIP_OPTION;
            if (cmbWorkshopCol.SelectedItem == null) cmbWorkshopCol.SelectedItem = SKIP_OPTION;
            if (cmbLevelCol.SelectedItem == null) cmbLevelCol.SelectedItem = SKIP_OPTION;
            if (cmbSquadCol.SelectedItem == null) cmbSquadCol.SelectedItem = SKIP_OPTION;
        }

        // Метод для обработки CSV-текста (из файла или из Google)
        private void ProcessCsvData(string csvContent)
        {
            try
            {
                // 1. Проверка на HTML (ошибка доступа Google)
                if (csvContent.TrimStart().StartsWith("<"))
                {
                    MessageBox.Show("Ошибка: Не удалось прочитать таблицу.\n\nУбедитесь, что в настройках Google Таблицы установлен доступ:\n'Все, у кого есть ссылка' -> 'Читатель'.", "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lblGoogleStatus.Text = "❌ Ошибка доступа";
                    return;
                }

                dataTable = new DataTable();
                string[] lines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length == 0)
                {
                    MessageBox.Show("Таблица пуста.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Умное определение разделителя (Google обычно использует ',', локальные CSV могут быть ';')
                char delimiter = lines[0].Contains(';') ? ';' : ',';

                string[] headers = lines[0].Split(delimiter);
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header.Trim());
                }

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] values = lines[i].Split(delimiter);
                    if (values.Length > 0 && !string.IsNullOrWhiteSpace(lines[i]))
                    {
                        dataTable.Rows.Add(values);
                    }
                }

                // 3. Обновляем интерфейс
                lblGoogleStatus.Text = "✅ Успешно загружено из Google";
                UpdateUIFromDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обработки CSV:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                        string ext = Path.GetExtension(ofd.FileName).ToLower();

                        if (ext == ".xlsx")
                        {
                            dataTable = new DataTable();
                            using (var workbook = new ClosedXML.Excel.XLWorkbook(ofd.FileName))
                            {
                                var worksheet = workbook.Worksheet(1);
                                var range = worksheet.RangeUsed();
                                if (range == null) throw new Exception("Лист Excel пуст.");

                                foreach (var cell in range.Row(1).Cells())
                                    dataTable.Columns.Add(cell.GetString().Trim());

                                for (int row = 2; row <= range.RowCount(); row++)
                                {
                                    var dataRow = dataTable.NewRow();
                                    int colIndex = 0;
                                    foreach (var cell in range.Row(row).Cells())
                                    {
                                        if (colIndex < dataTable.Columns.Count) dataRow[colIndex] = cell.GetString();
                                        colIndex++;
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                            lblGoogleStatus.Text = "✅ Загружено из файла";
                            UpdateUIFromDataTable();
                        }
                        else if (ext == ".csv")
                        {
                            string csvContent = File.ReadAllText(ofd.FileName, System.Text.Encoding.UTF8);
                            ProcessCsvData(csvContent);
                        }
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
            try
            {
                if (!string.IsNullOrEmpty(fontFilePath) && File.Exists(fontFilePath))
                {
                    // --- ВАРИАНТ А: Загрузка из файла (.ttf) ---
                    privateFonts = new PrivateFontCollection();
                    privateFonts.AddFontFile(fontFilePath);

                    fontFio = new Font(privateFonts.Families[0], (float)numFioSize.Value, FontStyle.Regular);
                    fontReg = new Font(privateFonts.Families[0], (float)numRegSize.Value, FontStyle.Regular);
                    fontHours = new Font(privateFonts.Families[0], (float)numHoursSize.Value, FontStyle.Regular);
                    fontWorkshop = new Font(privateFonts.Families[0], (float)numWorkshopSize.Value, FontStyle.Regular);
                    fontLevel = new Font(privateFonts.Families[0], (float)numLevelSize.Value, FontStyle.Regular);
                    fontSquad = new Font(privateFonts.Families[0], (float)numSquadSize.Value, FontStyle.Regular);
                    fontDate = new Font(privateFonts.Families[0], (float)numDateSize.Value, FontStyle.Regular);
                    fontMainText = new Font(privateFonts.Families[0], (float)numMainTextSize.Value, FontStyle.Regular);
                }
                else
                {
                    // --- ВАРИАНТ Б: Использование системного шрифта ---
                    string fontName = string.IsNullOrEmpty(selectedSystemFontName) ? "Arial" : selectedSystemFontName;

                    fontFio = new Font(fontName, (float)numFioSize.Value, FontStyle.Regular);
                    fontReg = new Font(fontName, (float)numRegSize.Value, FontStyle.Regular);
                    fontHours = new Font(fontName, (float)numHoursSize.Value, FontStyle.Regular);
                    fontWorkshop = new Font(fontName, (float)numWorkshopSize.Value, FontStyle.Regular);
                    fontLevel = new Font(fontName, (float)numLevelSize.Value, FontStyle.Regular);
                    fontSquad = new Font(fontName, (float)numSquadSize.Value, FontStyle.Regular);
                    fontDate = new Font(fontName, (float)numDateSize.Value, FontStyle.Regular);
                    fontMainText = new Font(fontName, (float)numMainTextSize.Value, FontStyle.Regular);
                }

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

                    using (SolidBrush textBrush = new SolidBrush(globalTextColor))
                    {
                        // 1. ФИО
                        if (fontFio != null && cmbFioCol.SelectedItem?.ToString() != SKIP_OPTION)
                        {
                            string fioText = "Иванов Иван Иванович";
                            if (dataTable != null && dataTable.Rows.Count > 0 && cmbFioCol.SelectedItem != null)
                                fioText = dataTable.Rows[0][cmbFioCol.SelectedItem.ToString()].ToString();

                            SizeF fioSize = g.MeasureString(fioText, fontFio);
                            float fioX = Math.Max(0, (backgroundImage.Width - fioSize.Width) / 2f);
                            g.DrawString(fioText, fontFio, textBrush, fioX, (float)numFioY.Value);
                        }

                        // 2. Регион
                        if (fontReg != null && cmbRegCol.SelectedItem?.ToString() != SKIP_OPTION)
                        {
                            string regText = "г. Москва";
                            if (dataTable != null && dataTable.Rows.Count > 0 && cmbRegCol.SelectedItem != null)
                            {
                                regText = dataTable.Rows[0][cmbRegCol.SelectedItem.ToString()].ToString();
                                regText = $"({regText})";
                            }


                            SizeF regSize = g.MeasureString(regText, fontReg);
                            float regX = Math.Max(0, (backgroundImage.Width - regSize.Width) / 2f);
                            g.DrawString(regText, fontReg, textBrush, regX, (float)numRegY.Value);
                        }

                        // 3. Часы (Тут без таблицы, но оставим проверку на всякий случай, если захотите расширить)
                        if (fontHours != null)
                        {
                            string hoursText = string.IsNullOrWhiteSpace(txtHours.Text) ? " " : txtHours.Text;
                            g.DrawString(hoursText, fontHours, textBrush, (float)numHoursX.Value, (float)numHoursY.Value);
                        }

                        // 4. Мастерская
                        if (fontWorkshop != null && cmbWorkshopCol.SelectedItem?.ToString() != SKIP_OPTION)
                        {
                            string workshopText = "Мастерская 'Золотые руки'";
                            if (dataTable != null && dataTable.Rows.Count > 0 && cmbWorkshopCol.SelectedItem != null)
                                workshopText = dataTable.Rows[0][cmbWorkshopCol.SelectedItem.ToString()].ToString();

                            SizeF workshopSize = g.MeasureString(workshopText, fontWorkshop);
                            float workshopX = Math.Max(0, (backgroundImage.Width - workshopSize.Width) / 2f);
                            g.DrawString(workshopText, fontWorkshop, textBrush, workshopX, (float)numWorkshopY.Value);
                        }

                        // 5. Уровень
                        if (fontLevel != null && cmbLevelCol.SelectedItem?.ToString() != SKIP_OPTION)
                        {
                            string levelText = "Продвинутый";
                            if (dataTable != null && dataTable.Rows.Count > 0 && cmbLevelCol.SelectedItem != null)
                                levelText = dataTable.Rows[0][cmbLevelCol.SelectedItem.ToString()].ToString();

                            g.DrawString(levelText, fontLevel, textBrush, (float)numLevelX.Value, (float)numLevelY.Value);
                        }

                        // 6. Отряд
                        if (fontSquad != null && cmbSquadCol.SelectedItem?.ToString() != SKIP_OPTION)
                        {
                            string squadText = "Отряд 'Лидер'";
                            if (dataTable != null && dataTable.Rows.Count > 0 && cmbSquadCol.SelectedItem != null)
                                squadText = dataTable.Rows[0][cmbSquadCol.SelectedItem.ToString()].ToString();

                            g.DrawString(squadText, fontSquad, textBrush, (float)numSquadX.Value, (float)numSquadY.Value);
                        }

                        // 7.Дата(Свободные координаты X и Y)
                        if (fontDate != null)
                        {
                            // Для предпросмотра показываем заглушку, если поле пустое, чтобы можно было выставить координаты
                            string dateText = string.IsNullOrWhiteSpace(txtDate.Text) ? "01.01.2024" : txtDate.Text;
                            g.DrawString(dateText, fontDate, textBrush, (float)numDateX.Value, (float)numDateY.Value);
                        }

                        // 8. Основной текст благодарности (Многострочный, по центру, с переносом)

                        if (fontMainText != null)
                        {
                            // Если поле пустое, показываем длинную заглушку для удобной настройки
                            string mainText = string.IsNullOrWhiteSpace(txtMainText.Text)
                                ? " "
                                : txtMainText.Text;

                            // Рассчитываем X для центрирования блока заданной ширины
                            float mainTextWidth = (float)numMainTextWidth.Value;
                            float mainTextX = Math.Max(0, (backgroundImage.Width - mainTextWidth) / 2f);

                            // Настраиваем форматирование: по центру горизонтально, прижато к верху вертикально
                            using (StringFormat sf = new StringFormat())
                            {
                                sf.Alignment = StringAlignment.Center;      // Центрируем каждую строку внутри блока
                                sf.LineAlignment = StringAlignment.Near;    // Прижимаем к верхней границе прямоугольника
                                sf.FormatFlags = StringFormatFlags.LineLimit; // Обрезаем, если вдруг текст гигантский

                                // Создаем прямоугольник отрисовки (Height = 2000, чтобы текст мог переноситься много раз)
                                RectangleF textRect = new RectangleF(mainTextX, (float)numMainTextY.Value, mainTextWidth, 2000f);

                                g.DrawString(mainText, fontMainText, textBrush, textRect, sf);
                            }
                        }
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

                                        using (SolidBrush textBrush = new SolidBrush(globalTextColor))
                                        {
                                            // --- ОТРИСОВКА ФИО ---
                                            if (cmbFioCol.SelectedItem?.ToString() != SKIP_OPTION)
                                            {
                                                string fioText = row[cmbFioCol.SelectedItem.ToString()].ToString();
                                                SizeF fioSize = g.MeasureString(fioText, fontFio);
                                                float fioX = Math.Max(0, (backgroundImage.Width - fioSize.Width) / 2f);
                                                g.DrawString(fioText, fontFio, textBrush, fioX, (float)numFioY.Value);
                                            }

                                            // --- ОТРИСОВКА РЕГИОНА ---
                                            if (cmbRegCol.SelectedItem?.ToString() != SKIP_OPTION)
                                            {
                                                string regText = row[cmbRegCol.SelectedItem.ToString()].ToString();
                                                regText = $"({regText})";
                                                SizeF regSize = g.MeasureString(regText, fontReg);
                                                float regX = Math.Max(0, (backgroundImage.Width - regSize.Width) / 2f);
                                                g.DrawString(regText, fontReg, textBrush, regX, (float)numRegY.Value);
                                            }

                                            // --- ОТРИСОВКА ЧАСОВ ---
                                            string hoursText = txtHours.Text;
                                            if (string.IsNullOrWhiteSpace(hoursText)) hoursText = " ";
                                            g.DrawString(hoursText, fontHours, textBrush, (float)numHoursX.Value, (float)numHoursY.Value);

                                            // --- ОТРИСОВКА МАСТЕРСКОЙ ---
                                            if (cmbWorkshopCol.SelectedItem?.ToString() != SKIP_OPTION)
                                            {
                                                string workshopText = row[cmbWorkshopCol.SelectedItem.ToString()].ToString();
                                                SizeF workshopSize = g.MeasureString(workshopText, fontWorkshop);
                                                float workshopX = Math.Max(0, (backgroundImage.Width - workshopSize.Width) / 2f);
                                                g.DrawString(workshopText, fontWorkshop, textBrush, workshopX, (float)numWorkshopY.Value);
                                            }

                                            // --- ОТРИСОВКА УРОВНЯ ---
                                            if (cmbLevelCol.SelectedItem?.ToString() != SKIP_OPTION)
                                            {
                                                string levelText = row[cmbLevelCol.SelectedItem.ToString()].ToString();
                                                g.DrawString(levelText, fontLevel, textBrush, (float)numLevelX.Value, (float)numLevelY.Value);
                                            }

                                            // --- ОТРИСОВКА ОТРЯДА ---
                                            if (cmbSquadCol.SelectedItem?.ToString() != SKIP_OPTION)
                                            {
                                                string squadText = row[cmbSquadCol.SelectedItem.ToString()].ToString();
                                                g.DrawString(squadText, fontSquad, textBrush, (float)numSquadX.Value, (float)numSquadY.Value);
                                            }

                                            // Дата (Рисуем ТОЛЬКО если поле не пустое)
                                            string dateText = txtDate.Text;
                                            if (!string.IsNullOrWhiteSpace(dateText) && fontDate != null)
                                            {
                                                g.DrawString(dateText, fontDate, textBrush, (float)numDateX.Value, (float)numDateY.Value);
                                            }
                                            // 8. Основной текст благодарности (Рисуем ТОЛЬКО если поле не пустое)
                                            // ==========================================
                                            string mainText = txtMainText.Text;
                                            if (!string.IsNullOrWhiteSpace(mainText) && fontMainText != null)
                                            {
                                                float mainTextWidth = (float)numMainTextWidth.Value;
                                                float mainTextX = Math.Max(0, (backgroundImage.Width - mainTextWidth) / 2f);

                                                using (StringFormat sf = new StringFormat())
                                                {
                                                    sf.Alignment = StringAlignment.Center;
                                                    sf.LineAlignment = StringAlignment.Near;
                                                    sf.FormatFlags = StringFormatFlags.LineLimit;

                                                    RectangleF textRect = new RectangleF(mainTextX, (float)numMainTextY.Value, mainTextWidth, 2000f);
                                                    g.DrawString(mainText, fontMainText, textBrush, textRect, sf);
                                                }
                                            }
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
            // 1. ПРОВЕРКА РЕЖИМА ПИПЕТКИ (Вставлено в самое начало!)
            if (isPipetteMode && backgroundImage != null)
            {
                // Получаем точные координаты на оригинальном изображении
                Point imgPoint = GetImageCoordinatesFromMouse(e.Location);

                // Забираем цвет пикселя
                globalTextColor = ((Bitmap)backgroundImage).GetPixel(imgPoint.X, imgPoint.Y);

                // Обновляем интерфейс
                pnlGlobalColor.BackColor = globalTextColor;
                lblStatus.Text = $"✅ Цвет выбран! (RGB: {globalTextColor.R}, {globalTextColor.G}, {globalTextColor.B})";

                // Сбрасываем режим и обновляем предпросмотр
                isPipetteMode = false;
                pbPreview.Cursor = Cursors.Default;
                UpdatePreview();

                return; // ВАЖНО: Выходим из метода, чтобы не сработала логика перемещения текста
            }

            // 2. Логика расстановки текста (Ваш старый код)
            if (activePlacementMode == null || backgroundImage == null) return;

            Point point = GetImageCoordinatesFromMouse(e.Location);

            if (activePlacementMode == "FIO")
            {
                numFioY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "REG")
            {
                numRegY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "HOURS")
            {
                numHoursX.Value = point.X;
                numHoursY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "WORKSHOP")
            {
                numWorkshopY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "LEVEL")
            {
                numLevelX.Value = point.X;
                numLevelY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "SQUAD")
            {
                numSquadX.Value = point.X;
                numSquadY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "DATE")
            {
                numDateX.Value = point.X;
                numDateY.Value = point.Y;
                activePlacementMode = null;
            }
            else if (activePlacementMode == "MAIN_TEXT")
            {
                numMainTextY.Value = point.Y; // Только Y, так как X центрируется автоматически
                activePlacementMode = null;
            }

            pbPreview.Cursor = Cursors.Default;
            lblStatus.Text = "✅ Координаты обновлены.";
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

        private void btnGlobalPipette_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон сертификата!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Включаем режим пипетки
            isPipetteMode = true;
            pbPreview.Cursor = Cursors.Cross; // Курсор-прицел
            lblStatus.Text = "🎯 Кликните по любому цвету на изображении сертификата";
        }

        private void btnPlaceDate_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "DATE";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы разместить Дату";
        }

        private void btnPlaceDate_Click_1(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "DATE";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы разместить Дату";
        }

        private void cmbFontSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbFontSelector.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selected)) return;

            if (selected == "📂 Загрузить свой файл шрифта (.ttf)...")
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Файлы шрифтов TrueType|*.ttf";
                    ofd.Title = "Выберите файл шрифта (.ttf)";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        fontFilePath = ofd.FileName;
                        selectedSystemFontName = ""; // Очищаем выбор системного шрифта
                        lblFontStatus.Text = "✅ Загружен файл: " + Path.GetFileName(fontFilePath);
                        LoadFonts();
                    }
                    else
                    {
                        // Пользователь нажал "Отмена", возвращаем предыдущий выбор (например, Arial)
                        cmbFontSelector.SelectedItem = string.IsNullOrEmpty(selectedSystemFontName) ? "Arial" : selectedSystemFontName;
                    }
                }
                // После диалога выходим, чтобы не сработала нижняя логика
                return;
            }
            else
            {
                // Выбран системный шрифт
                fontFilePath = ""; // Очищаем путь к файлу
                selectedSystemFontName = selected;
                lblFontStatus.Text = "✅ Системный шрифт: " + selectedSystemFontName;
                LoadFonts();
            }
        }

        private void btnPlaceMainText_Click(object sender, EventArgs e)
        {
            if (backgroundImage == null)
            {
                MessageBox.Show("Сначала загрузите фон!", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            activePlacementMode = "MAIN_TEXT";
            pbPreview.Cursor = Cursors.Cross;
            lblStatus.Text = "🎯 Кликните по изображению, чтобы задать высоту для основного текста";
        }

        private async void btnLoadGoogleSheet_Click(object sender, EventArgs e)
        {
            string url = txtGoogleSheetUrl.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Введите ссылку на Google Таблицу.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!url.Contains("docs.google.com/spreadsheets/d/"))
            {
                MessageBox.Show("Неверный формат ссылки. Убедитесь, что это ссылка на Google Таблицу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblGoogleStatus.Text = "⏳ Загрузка...";
            lblStatus.Text = "⏳ Загрузка данных из Google...";
            Application.DoEvents();

            try
            {
                // 1. Извлекаем ID таблицы
                Match idMatch = Regex.Match(url, @"/d/([a-zA-Z0-9-_]+)");
                if (!idMatch.Success) throw new Exception("Не удалось найти ID таблицы в ссылке.");
                string sheetId = idMatch.Groups[1].Value;

                // 2. Извлекаем GID вкладки (если есть, иначе 0)
                Match gidMatch = Regex.Match(url, @"gid=(\d+)");
                string gid = gidMatch.Success ? gidMatch.Groups[1].Value : "0";

                // 3. !!! ГЛАВНОЕ ИЗМЕНЕНИЕ: просим формат xlsx вместо csv !!!
                string exportUrl = $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=xlsx&gid={gid}";

                // 4. Скачиваем файл как массив байтов
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                    // Скачиваем байты Excel-файла
                    byte[] fileBytes = await client.GetByteArrayAsync(exportUrl);

                    // 5. Передаем байты в наш надежный Excel-парсер
                    ProcessGoogleExcelData(fileBytes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке из Google:\n" + ex.Message + "\n\nУбедитесь, что доступ к таблице открыт: 'Все, у кого есть ссылка' -> 'Читатель'.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblGoogleStatus.Text = "❌ Ошибка";
                lblStatus.Text = "Ошибка загрузки.";
            }
        }

        private void ProcessGoogleExcelData(byte[] fileBytes)
        {
            try
            {
                dataTable = new DataTable();

                // Читаем байты как поток Excel-файла
                using (var stream = new MemoryStream(fileBytes))
                {
                    using (var workbook = new ClosedXML.Excel.XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1);
                        var range = worksheet.RangeUsed();

                        if (range == null)
                        {
                            MessageBox.Show("Лист Google Таблицы пуст.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // 1. Создаем колонки из первой строки
                        foreach (var cell in range.Row(1).Cells())
                        {
                            dataTable.Columns.Add(cell.GetString().Trim());
                        }

                        // 2. Заполняем строки данными (ClosedXML сам корректно обработает запятые и переносы внутри ячеек!)
                        for (int row = 2; row <= range.RowCount(); row++)
                        {
                            var dataRow = dataTable.NewRow();
                            int colIndex = 0;

                            foreach (var cell in range.Row(row).Cells())
                            {
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

                // 3. Обновляем интерфейс
                lblGoogleStatus.Text = "✅ Успешно загружено из Google (Excel)";
                UpdateUIFromDataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обработки данных из Google:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblGoogleStatus.Text = "❌ Ошибка обработки";
            }
        }
    }


}


