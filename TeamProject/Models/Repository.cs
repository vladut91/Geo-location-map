using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml;
using System.Data;

namespace TeamProject.Models
{
    public class Repository
    {
        private locationDataContext db = new locationDataContext();

        //
        // Query methods

        public IQueryable<geoLocation> AllLocations()
        {
            return db.geoLocations;
        }

        public geoLocation GetLocationDetails(int id)
        {
            return db.geoLocations.SingleOrDefault(d => d.ID == id);
        }


        //
        // Insert/Delete Methods

        public void Add(geoLocation address)
        {
            db.geoLocations.InsertOnSubmit(address);
        }

        public void Delete(geoLocation address)
        {
            db.geoLocations.DeleteOnSubmit(address);
        }


        //
        // Persistance

        public void Save()
        {
            db.SubmitChanges();
        }


        //
        // Find IP from URL

        public string FindIpAddress(string ip)
        {
            IPHostEntry IP_URL = Dns.GetHostEntry(ip);
            IPAddress[] IpA = IP_URL.AddressList;

            return IpA[0].ToString();
        }

        public DataTable GetGeoLocationByIP(string ipaddress)
        {

            //Create a WebRequest

            WebRequest rssReq =

                WebRequest.Create("http://ws.cdyne.com/ip2geo/ip2geo.asmx/ResolveIP?ipAddress="

                    + ipaddress + "&licenseKey=0");



            //Create a Proxy

            WebProxy px =

               new WebProxy("http://ws.cdyne.com/ip2geo/ip2geo.asmx/ResolveIP?ipAddress="

                    + ipaddress + "&licenseKey=0", true);



            //Assign the proxy to the WebRequest

            rssReq.Proxy = px;



            //Set the timeout in Seconds for the WebRequest

            rssReq.Timeout = 2000;

            try
            {

                //Get the WebResponse

                WebResponse rep = rssReq.GetResponse();



                //Read the Response in a XMLTextReader

                XmlTextReader xtr = new XmlTextReader(rep.GetResponseStream());



                //Create a new DataSet

                DataSet ds = new DataSet();



                //Read the Response into the DataSet

                ds.ReadXml(xtr);

                return ds.Tables[0];

            }

            catch
            {

                return null;

            }

        }
    }
}
