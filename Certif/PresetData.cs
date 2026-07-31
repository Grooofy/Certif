using System;

namespace Certif
{
    public class PresetData
    {
        // Имя пресета (для удобства)
        public string PresetName { get; set; } = "";

        // Координаты ФИО       
        public decimal FioY { get; set; }
        public decimal FioSize { get; set; }

        // Координаты Региона        
        public decimal RegY { get; set; }
        public decimal RegSize { get; set; }

        // Координаты Часов
        public decimal HoursX { get; set; }
        public decimal HoursY { get; set; }
        public decimal HoursSize { get; set; }
        public string HoursText { get; set; } = "";

        // Координаты Мастерской       
        public decimal WorkshopY { get; set; }
        public decimal WorkshopSize { get; set; }

        // Координаты Уровня
        public decimal LevelX { get; set; }
        public decimal LevelY { get; set; }
        public decimal LevelSize { get; set; }

        // Координаты Отряда
        public decimal SquadX { get; set; }
        public decimal SquadY { get; set; }
        public decimal SquadSize { get; set; }

        // Координаты Даты
        public decimal DateX { get; set; }
        public decimal DateY { get; set; }
        public decimal DateSize { get; set; }
        public string DateText { get; set; } = "";

        // Основной текст благодарности
        public decimal MainTextY { get; set; }
        public decimal MainTextSize { get; set; }
        public decimal MainTextWidth { get; set; }
        public string MainText { get; set; } = "";

        // Шрифт
        public string FontFileName { get; set; } = "";
        public string SystemFontName { get; set; } = "Arial";

        // Цвет текста (RGB)
        public int ColorR { get; set; } = 0;
        public int ColorG { get; set; } = 0;
        public int ColorB { get; set; } = 0;

        // Выбранные колонки из таблицы
        public string FioColumnName { get; set; } = "";
        public string RegColumnName { get; set; } = "";
        public string WorkshopColumnName { get; set; } = "";
        public string LevelColumnName { get; set; } = "";
        public string SquadColumnName { get; set; } = "";

        // Путь к фоновому изображению (опционально)
        public string BackgroundImagePath { get; set; } = "";
    }
}