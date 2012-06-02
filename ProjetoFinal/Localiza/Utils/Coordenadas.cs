
namespace Utils
{
    /// <summary>
    /// Cordenadas. Contem Latitude and Longitude.
    /// </summary>
    public struct Coordenadas
    {
        private decimal latitude;
        private decimal longitude;

        public Coordenadas(decimal _lat, decimal _long)
        {
            latitude = _lat;
            longitude = _long;
        }

        public decimal Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                this.latitude = value;
            }
        }

        public decimal Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                this.longitude = value;
            }
        }
    }
}
