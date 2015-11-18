using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using TeamProject.Models;
using System.Data;

namespace TeamProject.Controllers
{
    public class HomeController : Controller
    {
        Repository addressObj = new Repository();


        //
        // GET:/Home/Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // POST: /Home/Index
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string urlEnter, geoLocation location)
        {
            var address = addressObj.AllLocations().ToList();
            var url = Request.Form["urlEnter"];
            int x = 0;
            bool found = false;
            string Locations = "";
            while (x != address.Count)
            {
                if (address[x].URL == url)
                {
                    found = true;
                    break;
                }

                x++;
            }
            if (found)
            {
                ViewData["URL"] = address[x].URL;
                ViewData["IP"] = address[x].IP;
                ViewData["Latitude"] = address[x].Latitude;
                ViewData["Longitude"] = address[x].Longitute;
                Locations += Environment.NewLine + " map.addOverlay(new GMarker(new GLatLng(" + address[x].Latitude + "," + address[x].Longitute + ")));";
            }
            else
            {
                var ip = addressObj.FindIpAddress(urlEnter);

                if (ip != null)
                {
                    DataTable dt = addressObj.GetGeoLocationByIP(ip);
                    location.IP = ip;
                    location.URL = urlEnter;
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 12)
                        {
                            location.City = dt.Rows[0]["City"].ToString();
                            location.Region = dt.Rows[0]["StateProvince"].ToString();
                        }
                        location.Country = dt.Rows[0]["Country"].ToString();
                        location.CountryCode = dt.Rows[0]["CountryCode"].ToString();
                        location.Latitude = dt.Rows[0]["Latitude"].ToString();
                        location.Longitute = dt.Rows[0]["Longitude"].ToString();
                        addressObj.Add(location);
                        addressObj.Save();

                        ViewData["URL"] = location.URL;
                        ViewData["IP"] = location.IP;
                        ViewData["Latitude"] = location.Latitude;
                        ViewData["Longitude"] = location.Longitute;
                       // Locations += Environment.NewLine + " map.addOverlay(new GMarker(new GLatLng(" + location.Latitude + "," + location.Longitute + ")));";
                    }

                    else
                    {
                    }

                }
            }

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult About()
        {
            var address = addressObj.AllLocations().ToList();
            return View(address);
        }
        
        
        //
        // GET:/Home/About/Details

        public ActionResult Details(int id)
        {
            geoLocation location = addressObj.GetLocationDetails(id);

            return View(location);
        }


    }
}
