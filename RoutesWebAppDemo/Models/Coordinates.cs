using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoutesWebAppDemo.Models
{
    public class Coordinates
    {
        public double lat { get; set; }
        public double lng { get; set; }

        public Coordinates(double lat, double lng)
        {
            this.lat = lat;
            this.lng = lng;
        }

        public Coordinates()
        {
            this.lat = 47.260705;
            this.lng = -122.482980;
        }

        public static List<Coordinates> GenerateRandCor(Coordinates cor, double radius, int n)
        {
            var cors = new List<Coordinates>();
            var rand = new Random();
            double minDist = 0.4;

            while (cors.Count < n)
            {
                var r = rand.NextDouble() * radius; // random radius
                var t = rand.NextDouble() * 2 * Math.PI; // random theta in radians
                // calculate new latitude and longtitude based on distance
                var cLat = cor.lat / 180 * Math.PI; // convert to radians
                var cLng = cor.lng / 180 * Math.PI;
                var newLat = (Math.Asin(Math.Sin(cLat) * Math.Cos(r / 6378.1) + Math.Cos(cLat) * Math.Sin(r / 6378.1) * Math.Cos(t))) * 180 / Math.PI;
                var newLng = (cLng + Math.Atan2(Math.Sin(t) * Math.Sin(r / 6378.1) * Math.Cos(cLat), Math.Cos(r / 6378.1) - Math.Sin(cLat) * Math.Sin(newLat))) * 180 / Math.PI;

                var point = new Coordinates(newLat, newLng); // create object
                var sqlPoint = SqlGeography.Point(point.lat, point.lng, 4326);
                bool overlap = false;
                foreach (var c in cors) // check distance with existing points
                {
                    var other = SqlGeography.Point(c.lat, c.lng, 4326);
                    if (sqlPoint.STDistance(other) / 1000 < minDist) // assuming STDistance returns distance in meters
                    {
                        overlap = true;
                        break;
                    }
                }
                if (!overlap)
                {
                    cors.Add(point);
                }
            }

            return cors;
        }

        override
        public String ToString()
        {
            return lat + "," + lng;
        }
    }
}