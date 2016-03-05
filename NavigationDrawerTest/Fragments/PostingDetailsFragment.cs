using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using EthansList.Models;
using EthansList.Shared;

namespace EthansList.MaterialDroid
{
    public class PostingDetailsFragment : Android.Support.V4.App.Fragment
    {
        public Posting Posting { get; set; }
        private ListingImageDownloader imageHelper;

        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = new PostingDetailsView(this.Activity);

            view.PostingTitle.Text = Posting.PostTitle;

            if (Posting.ImageLink != "-1")
            {
                view.CurrentImage = Posting.ImageLink;
            }

            imageHelper = new ListingImageDownloader(Posting.Link, Posting.ImageLink);
            var connected = imageHelper.GetAllImagesAsync();
            if (!connected)
            { 
                //TODO: handle no network connection here
                Console.WriteLine("Not connectioned to network");
            }

            imageHelper.loadingComplete += (sender, e) =>
            {
                if (imageHelper.PostingImagesFound)
                {
                    view.ImageCollection.SetNumColumns(imageHelper.images.Count);
                    //view.ImageCollection.SetColumnWidth(150);
                    view.ImageCollection.Adapter = new ImageAdapter(this.Activity, imageHelper.images);
                }

                if (imageHelper.PostingBodyAdded)
                    view.PostingDescription.Text = imageHelper.postingDescription;

                if (imageHelper.PostingMapFound)
                {
                    if (view.SetUpGoogleMap())
                    {
                        view.MapReady += delegate {
                            //To initialize the map 
                            view.map.MapType = GoogleMap.MapTypeNormal; //select the map type
                            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                            builder.Target(imageHelper.postingCoordinates); //Target to some location hardcoded
                            builder.Zoom(8); //Zoom multiplier
                            CameraPosition cameraPosition = builder.Build();
                            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                            view.map.AnimateCamera(cameraUpdate);

                            MarkerOptions markerOpt1 = new MarkerOptions();
                            markerOpt1.SetPosition(imageHelper.postingCoordinates);
                            markerOpt1.SetTitle("Here's your listing!");
                            view.map.AddMarker(markerOpt1);
                        };
                    }
                }
            };

            view.ImageCollection.ItemClick += (Object sender, AdapterView.ItemClickEventArgs args) => {
                view.CurrentImage = imageHelper.images.ElementAt(args.Position);
            };

            view.PostingDescription.Text = Posting.Description;
            view.PostingDate.Text = "Listed: " + Posting.Date.ToShortDateString() + " at " + Posting.Date.ToShortTimeString();
            view.WebLink.Text = Posting.Link;

            view.WebLink.Click += (sender, e) => {
                var intent = new Intent();
                intent.SetAction(Intent.ActionView);
                intent.SetData(Android.Net.Uri.Parse(Posting.Link));
                StartActivity(intent);
            };

            ScrollView viewContainer = new ScrollView(this.Activity);
            viewContainer.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            viewContainer.AddView(view);
            return viewContainer;
        }
    }

    //OnMapReadyClass
    public class OnMapReadyClass : Java.Lang.Object, IOnMapReadyCallback
    { 
        public GoogleMap Map { get; private set; }
        public event Action<GoogleMap> MapReadyAction;

        public void OnMapReady (GoogleMap googleMap)
        {
            Map = googleMap; 

            if ( MapReadyAction != null )
                MapReadyAction (Map);
        }
    }

    public class PostingDetailsView : LinearLayout
    {
        readonly Context _context;
        //readonly LayoutParams rowParams;
        readonly LayoutParams textRowParams;

        public TextView PostingTitle { get; set; }
        public ImageView PostingImage { get; set; }
        public GridView ImageCollection { get; set; }
        public TextView PostingDescription { get; set; }
        public TextView PostingDate { get; set; }

        //todo: posting map and weblink
        public MapView PostingMap { get; set; }
        public TextView WebLink { get; set; }

        public GoogleMap map { get; set; }
        public bool SetUpGoogleMap()
        {
            if(null != map ) return false;

            var mapReadyCallback = new OnMapReadyClass();

            mapReadyCallback.MapReadyAction += delegate(GoogleMap googleMap )
            {
                map = googleMap;
                if (this.MapReady != null)
                    this.MapReady(this, new EventArgs());
            };

            PostingMap.GetMapAsync(mapReadyCallback); 
            return true;
        }

        public event EventHandler<EventArgs> MapReady;

        public string CurrentImage
        {
            get { return currentImage; }
            set { 
                Koush.UrlImageViewHelper.SetUrlDrawable(PostingImage, value, Resource.Drawable.placeholder);
                currentImage = value;
            }
        }
        string currentImage;

        public PostingDetailsView(Context context)
            :base (context)
        {
            _context = context;
            //rowParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            textRowParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            WeightSum = 1;

            PostingTitle = new TextView(_context) { LayoutParameters = textRowParams};
            PostingTitle.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            PostingTitle.SetPadding(10, 10, 10, 10);
            PostingTitle.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            AddRowItem(PostingTitle, textRowParams);

            PostingImage = new ImageView(_context) { LayoutParameters = textRowParams};
            AddRowItem(PostingImage, textRowParams);

            //TODO fix this fuckiness
            ImageCollection = new GridView(_context);
            ImageCollection.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            ImageCollection.SetColumnWidth(150);
            ImageCollection.StretchMode = StretchMode.StretchColumnWidth;
            ImageCollection.NumColumns = (int)StretchMode.AutoFit;
            ScrollView imageScroller = new ScrollView(_context);
            imageScroller.AddView(ImageCollection); 
            AddRowItem(imageScroller, new LayoutParams(LayoutParams.WrapContent, 150));

            PostingDescription = new TextView(_context) { LayoutParameters = textRowParams };
            PostingDescription.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            PostingDescription.SetPadding(10, 10, 10, 10);
            PostingDescription.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
            AddRowItem(PostingDescription, textRowParams);

            PostingMap = new MapView(_context);
            //AddRowItem(PostingMap, new LayoutParams(ViewGroup.LayoutParams.MatchParent, 300));

            PostingDate = new TextView(_context) { LayoutParameters = textRowParams };
            PostingDate.SetPadding(10, 10, 10, 10);
            PostingDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            AddRowItem(PostingDate, textRowParams);

            WebLink = new TextView(_context) { LayoutParameters = textRowParams };
            WebLink.SetPadding(10, 10, 10, 10);
            WebLink.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            AddRowItem(WebLink, textRowParams);
        }

        private void AddRowItem(View item, LayoutParams par)
        {
            var view = new LinearLayout(_context) { Orientation = Orientation.Horizontal };
            view.LayoutParameters = par;
            view.AddView(item);

            AddView(view);
        }
    }
}

