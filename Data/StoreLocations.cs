using System;

namespace BakeryBI.Data
{
    public class StoreLocation
    {
        public string StoreName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public decimal TotalSales { get; set; }
        public int TransactionCount { get; set; }

        public static (double lat, double lon) GetCityCoordinates(string city)
        {
            return city?.Trim() switch
            {
                "Bucuresti" or "București" => (44.4268, 26.1025),
                "Cluj-Napoca" => (46.7712, 23.6236),
                "Iasi" or "Iași" => (47.1585, 27.6014),
                "Timisoara" or "Timișoara" => (45.7489, 21.2087),
                "Constanta" or "Constanța" => (44.1598, 28.6348),
                "Brasov" or "Brașov" => (45.6579, 25.6012),
                "Craiova" => (44.3302, 23.7949),
                "Sibiu" => (45.7983, 24.1256),
                "Oradea" => (47.0722, 21.9211),
                _ => (45.9432, 24.9668)
            };
        }
    }
}