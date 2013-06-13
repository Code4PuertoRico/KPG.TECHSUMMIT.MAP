using Bing.Maps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;//To show popups with some message.
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KPG.TECHSUMMIT.MAP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Pushpin pin;
        private Geolocator _geolocator;

        public MainPage()
        {
            
            this.InitializeComponent();
            _geolocator = new Geolocator();
            _geolocator.DesiredAccuracy = PositionAccuracy.High;
            _geolocator.PositionChanged += new TypedEventHandler<Geolocator, PositionChangedEventArgs>(OnPositionChanged);
            //map.SetView(new Location(18.264015, -66.430797), 10);//To set the startup location of the map via code.
            //map.MapType = MapType.Road; //To change the map style via code.

            //Geolocation pushpin
            pin = new Pushpin
            {
                Background = new SolidColorBrush(Colors.Red)
               
            };
            map.Children.Add(pin);

            Rect display = Window.Current.Bounds;

            map.Width = display.Width;
            map.Height = display.Height;

            double cbPosX = (map.Width / 2) - (cbMunicipalities.Width / 2);
            double cbPosY = (map.Height / 4) * 3.5 - (cbMunicipalities.Height / 2);

            cbMunicipalities.Margin = new Thickness(cbPosX, cbPosY, 0, 0);
            
           
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Find the geolocation pin
            FindCurrentLocation();
        }

        

        private async void getCoordinatesFromAPI(string municipality)
        {
            try
            {
                string API_BaseAddress = "https://servicios.adsef.pr.gov/";
                //string API_BaseAddress = "http://10.201.11.2/";
                string API_Address = "sdec_wapi/api/Establishments/?Status=activo&establishment=*&municipality=" + municipality;

                HttpClient client = new HttpClient();
                //DA.OBJ_Establishment _establishments = new OBJ_Establishment();

                client.BaseAddress = new Uri(API_BaseAddress);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(API_Address);
                response.EnsureSuccessStatusCode(); // Throw on error code.

                //get json
                string json = await response.Content.ReadAsStringAsync();
                var jArray = JArray.Parse(json);
                //Convert jArray to a list of establishment objects
                List<OBJ_Establishment> x = jArray.ToObject<List<OBJ_Establishment>>();

                foreach (OBJ_Establishment oEst in x)
                {
                    if (!string.IsNullOrEmpty(oEst.estLatLong))
                    {
                        string[] latLon = oEst.estLatLong.Split(',');

                        AddPushpinToMap(Convert.ToDouble(latLon[0]), Convert.ToDouble(latLon[1]), oEst);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        /// <summary>
        /// This is the event handler for PositionChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Geoposition position = e.Position;

                var locationFound = new Location
                {
                    Latitude = position.Coordinate.Latitude,
                    Longitude = position.Coordinate.Longitude
                };

                MapLayer.SetPosition(pin, locationFound);
                map.SetView(locationFound, 15.0f);

            });
        }
        /// <summary>
        /// This is the event handler for StatusChanged events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //async private void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    {
        //        switch (e.Status)
        //        {
        //            case PositionStatus.Ready:
        //                // Location platform is providing valid data.
        //                //ScenarioOutput_Status.Text = "Ready";
        //                break;

        //            case PositionStatus.Initializing:
        //                // Location platform is acquiring a fix. It may or may not have data. Or the data may be less accurate.
        //                //ScenarioOutput_Status.Text = "Initializing";
        //                break;

        //            case PositionStatus.NoData:
        //                // Location platform could not obtain location data.
        //                //ScenarioOutput_Status.Text = "No data";
        //                break;

        //            case PositionStatus.Disabled:
        //                // The permission to access location data is denied by the user or other policies.
        //                //ScenarioOutput_Status.Text = "Disabled";

        //                //Clear cached location data if any
        //                //ScenarioOutput_Latitude.Text = "No data";
        //                //ScenarioOutput_Longitude.Text = "No data";
        //                //ScenarioOutput_Accuracy.Text = "No data";
        //                break;

        //            case PositionStatus.NotInitialized:
        //                // The location platform is not initialized. This indicates that the application has not made a request for location data.
        //                //ScenarioOutput_Status.Text = "Not initialized";
        //                break;

        //            case PositionStatus.NotAvailable:
        //                // The location platform is not available on this version of the OS.
        //                //ScenarioOutput_Status.Text = "Not available";
        //                break;

        //            default:
        //                //ScenarioOutput_Status.Text = "Unknown";
        //                break;
        //        }
        //    });
        //}

        //Add a pushpin with a label to the map
        private void AddPushpinToMap(double latitude, double longitude, OBJ_Establishment oEst)
        {
            Location location = new Location(latitude, longitude);
            Pushpin pushpin = new Pushpin();
            pushpin.Text = oEst.estName;
            pushpin.Tag = oEst;
            pushpin.Tapped += new TappedEventHandler(pushpinTapped);//Add onclick pushpin event
            MapLayer.SetPosition(pushpin, new Location(location));
            map.Children.Add(pushpin);
        }

        private void cbMunicipalities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getCoordinatesFromAPI(this.cbMunicipalities.SelectedItem.ToString());
        }

        /// <summary>
        /// Show popup messages
        /// </summary>
        private async void pushpinTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Pushpin pushpin = (Pushpin)sender;
            OBJ_Establishment oEst = (OBJ_Establishment)pushpin.Tag;
            MessageDialog dialog = new MessageDialog("Comercio: " + oEst.estName + "\n" + 
                                                    "Email: " + oEst.estEmail + "\n" +
                                                    "Estatus: " + oEst.estStatus + "\n" +
                                                    "Dirección: " + oEst.estAddPhysical1 + "\n" +
                                                    "                 " + oEst.estAddPhysical2 + "\n" +
                                                    "                 " + oEst.estAddPhysicalCity + ", " + oEst.estAddPhysicalState + ", " + oEst.estAddPhysicalZipCode + "\n"
            );
            await dialog.ShowAsync();
        }

        async private void FindCurrentLocation()
        {
            
            var currentPosition = await _geolocator.GetGeopositionAsync();

           

            var locationFound = new Location
            {
                Latitude = currentPosition.Coordinate.Latitude,
                Longitude = currentPosition.Coordinate.Longitude
            };

            MapLayer.SetPosition(pin, locationFound);
            map.SetView(locationFound, 15.0f);
        }
    }
}
