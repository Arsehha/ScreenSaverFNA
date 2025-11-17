namespace ScreenSaver.Classes
{
    /// <summary>
    /// Класс описывающий параметры снежинок
    /// </summary>
    public class Snowflake
    {
        /// <summary>
        /// Местоположение снежинки по X
        /// </summary>
        public float X { set; get; }

        /// <summary>
        /// Местоположение снежинки по Y
        /// </summary>
        public float Y { set; get; }

        /// <summary>
        /// Размер снежинки
        /// </summary>
        public int Size { set; get; }

        /// <summary>
        /// Скорость снежинки
        /// </summary>
        public float Speed { set; get; }
    }
}
