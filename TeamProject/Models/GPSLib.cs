using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace TeamProject.Models
{
    public class GPSLib
    {
        public static String PlotGPSPoints(DataTable tblPoints)
        {
            try
            {
                String Locations = "";
                String sJScript = "";
                int i = 0;
                foreach (DataRow r in tblPoints.Rows)
                {
                    // bypass empty rows 
                    if (r["latitude"].ToString().Trim().Length == 0)
                        continue;

                    string Latitude = r["latitude"].ToString();
                    string Longitude = r["longitude"].ToString();

                    // create a line of JavaScript for marker on map for this record 
                    Locations += Environment.NewLine + @"
                path.push(new google.maps.LatLng(" + Latitude + ", " + Longitude + @"));

                var marker" + i.ToString() + @" = new google.maps.Marker({
                    position: new google.maps.LatLng(" + Latitude + ", " + Longitude + @"),
                    title: '#' + path.getLength(),
                    map: map
                });";
                    i++;
                }

                // construct the final script
                sJScript = @"<script type='text/javascript'>

            var poly;
            var map;

            function initialize() {
                var cmloc = new google.maps.LatLng(1, -1);
                var myOptions = {
                    zoom: 11,
                    center: cmloc,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                map = new google.maps.Map(document.getElementById('map_canvas'), myOptions);

                var polyOptions = {
                    strokeColor: 'blue',
                    strokeOpacity: 0.5,
                    strokeWeight: 3
                }
                poly = new google.maps.Polyline(polyOptions);
                poly.setMap(map);

                var path = poly.getPath();

               " + Locations + @"

                    }
                </script>";
                return sJScript;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}