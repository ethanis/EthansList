using System;
using System.Collections.Generic;
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

namespace EthansList.Droid
{
    public class PostingDetailsFragment : Android.Support.V4.App.Fragment
    {
        public Posting Posting { get; set; }
        private ListingImageDownloader imageHelper;

        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
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
                var builder = new Android.Support.V7.App.AlertDialog.Builder(this.Activity);

                builder.SetTitle("No internet connection")
                    .SetMessage("Please connect and try again")
                    .SetPositiveButton("Ok", delegate
                    {
                        Console.WriteLine("Not connected");
                    });

                builder.Create().Show();
            }

            imageHelper.loadingComplete += (sender, e) =>
            {
                if (imageHelper.PostingImagesFound)
                {
                    view.SetImageCollection(imageHelper.images);
                }

                if (imageHelper.PostingBodyAdded)
                    view.PostingDescription.Text = imageHelper.postingDescription;

                if (imageHelper.PostingMapFound)
                {

                    SupportMapFragment _myMapFragment = SupportMapFragment.NewInstance();
                    view.mapFrame.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent,
                                                                                   Convert.ToInt16(Activity.Resources.DisplayMetrics.HeightPixels * 0.5));
                    var tx = Activity.SupportFragmentManager.BeginTransaction();
                    tx.Replace(view.mapFrame.Id, _myMapFragment);
                    tx.Commit();

                    SetUpGoogleMap(_myMapFragment);

                    this.MapReady += (s, ev) =>
                    {
                        if (map != null)
                        {
                            //To initialize the map 
                            map.MapType = GoogleMap.MapTypeNormal; //select the map type
                            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                            builder.Target(imageHelper.postingCoordinates); //Target to some location hardcoded
                            builder.Zoom(10); //Zoom multiplier
                            CameraPosition cameraPosition = builder.Build();
                            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
                            //map.AnimateCamera(cameraUpdate);
                            map.MoveCamera(cameraUpdate);

                            MarkerOptions markerOpt1 = new MarkerOptions();
                            markerOpt1.SetPosition(imageHelper.postingCoordinates);
                            markerOpt1.SetTitle("Here's your listing!");
                            map.AddMarker(markerOpt1);
                        }
                    };
                }
            };

            view.ImageClick += (sender, e) =>
            {
                view.CurrentImage = imageHelper.images.ElementAt(e.Index);
            };

            view.PostingDescription.Text = Posting.Description;
            view.PostingDate.Text = "Listed: " + Posting.Date.ToShortDateString() + " at " + Posting.Date.ToShortTimeString();
            view.WebLink.Text = "Original Listing";

            view.WebLink.Click += (sender, e) =>
            {
                var transaction = Activity.SupportFragmentManager.BeginTransaction();
                WebviewFragment webviewFragment = new WebviewFragment();
                webviewFragment.Link = Posting.Link;

                transaction.Replace(Resource.Id.frameLayout, webviewFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            view.PostingImage.Click += (sender, e) =>
            {
                var transaction = Activity.SupportFragmentManager.BeginTransaction();
                ImageZoomFragment zoomFragment = new ImageZoomFragment();
                zoomFragment.ImageUrl = view.CurrentImage;

                transaction.Replace(Resource.Id.frameLayout, zoomFragment);
                transaction.AddToBackStack(null);
                transaction.Commit();
            };

            ((MainActivity)this.Activity).OptionItemSelected += OnOptionItemSelected;

            ScrollView viewContainer = new ScrollView(this.Activity);
            viewContainer.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

            viewContainer.AddView(view);
            return viewContainer;
        }

        async void OnOptionItemSelected(object sender, OptionItemEventArgs e)
        {
            if (e.Item.TitleFormatted.ToString() == "Save")
            {
                await MainActivity.databaseConnection.AddNewListingAsync(Posting.PostTitle,
                                                                   Posting.Description,
                                                                   Posting.Link,
                                                                   Posting.ImageLink,
                                                                   Posting.Date);

                if (MainActivity.databaseConnection.StatusCode == codes.ok)
                    Toast.MakeText(this.Activity, string.Format("Saved Posting!"), ToastLength.Short).Show();
                else
                    Toast.MakeText(this.Activity, string.Format("Oops, something went wrong"), ToastLength.Short).Show();
            }
        }

        public override void OnPrepareOptionsMenu(IMenu menu)
        {
            IMenuItem save_item = menu.FindItem(Resource.Id.save_action_button);
            save_item.SetVisible(true);

            base.OnPrepareOptionsMenu(menu);
        }

        GoogleMap map;
        bool SetUpGoogleMap(SupportMapFragment mapFrag)
        {
            if (null != map) return false;

            var mapReadyCallback = new OnMapReadyClass();

            mapReadyCallback.MapReadyAction += delegate (GoogleMap googleMap)
            {
                map = googleMap;
                if (MapReady != null)
                    this.MapReady(this, new EventArgs());
            };

            mapFrag.GetMapAsync(mapReadyCallback);
            return true;
        }

        public event EventHandler<EventArgs> MapReady;
    }

    //OnMapReadyClass
    public class OnMapReadyClass : Java.Lang.Object, IOnMapReadyCallback
    {
        public GoogleMap Map { get; private set; }
        public event Action<GoogleMap> MapReadyAction;

        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;

            if (MapReadyAction != null)
                MapReadyAction(Map);
        }
    }

    public class PostingDetailsView : LinearLayout
    {
        readonly Context _context;
        readonly LayoutParams rowParams;

        public TextView PostingTitle { get; set; }
        public ImageView PostingImage { get; set; }
        public TextView PostingDescription { get; set; }
        public TextView PostingDate { get; set; }
        public LinearLayout ImageCollectionHolder { get; set; }
        public FrameLayout mapFrame { get; set; }
        public TextView WebLink { get; set; }

        public string CurrentImage
        {
            get { return currentImage; }
            set
            {
                Koush.UrlImageViewHelper.SetUrlDrawable(PostingImage, value, Resource.Drawable.placeholder);
                currentImage = value;
            }
        }
        string currentImage;

        public PostingDetailsView(Context context)
            : base(context)
        {
            _context = context;
            rowParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            Initialize();
        }

        void Initialize()
        {
            Orientation = Orientation.Vertical;
            WeightSum = 1;

            PostingTitle = new TextView(_context) { LayoutParameters = rowParams };
            PostingTitle.SetTextSize(Android.Util.ComplexUnitType.Dip, 18);
            PostingTitle.SetPadding(10, 10, 10, 10);
            PostingTitle.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
            AddRowItem(PostingTitle, rowParams);

            PostingImage = new ImageView(_context);// { LayoutParameters = rowParams };
            PostingImage.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent,
                                                                       Convert.ToInt16(_context.Resources.DisplayMetrics.HeightPixels * 0.4));
            AddRowItem(PostingImage, rowParams);

            LinearLayout spacing = new LinearLayout(_context);
            spacing.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, PixelConverter.DpToPixels(5));

            AddView(spacing);

            ImageCollectionHolder = new LinearLayout(_context) { Orientation = Orientation.Horizontal };
            ImageCollectionHolder.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            ImageCollectionHolder.Clickable = false;
            HorizontalScrollView scrollView = new HorizontalScrollView(_context);
            scrollView.AddView(ImageCollectionHolder);
            scrollView.Clickable = false;
            AddView(scrollView);

            PostingDescription = new TextView(_context) { LayoutParameters = rowParams };
            PostingDescription.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            PostingDescription.SetPadding(10, 10, 10, 10);
            PostingDescription.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
            AddRowItem(PostingDescription, rowParams);

            mapFrame = new FrameLayout(_context);
            mapFrame.Id = 12345;
            AddView(mapFrame);

            PostingDate = new TextView(_context) { LayoutParameters = rowParams };
            PostingDate.SetPadding(10, 10, 10, 10);
            PostingDate.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            AddRowItem(PostingDate, rowParams);

            WebLink = new TextView(_context) { LayoutParameters = rowParams };
            WebLink.SetPadding(10, 10, 10, 10);
            WebLink.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);
            AddRowItem(WebLink, rowParams);
        }

        private void AddRowItem(View item, LayoutParams par)
        {
            var view = new LinearLayout(_context) { Orientation = Orientation.Horizontal };
            view.LayoutParameters = par;
            view.AddView(item);

            AddView(view);
        }

        public event EventHandler<ImageCollectionClickEventArgs> ImageClick;

        public void SetImageCollection(List<string> images)
        {
            int index = 0;
            foreach (var image in images)
            {
                ImageView imgView = new ImageView(_context);
                Koush.UrlImageViewHelper.SetUrlDrawable(imgView, image, Resource.Drawable.placeholder);
                imgView.LayoutParameters = new ViewGroup.LayoutParams(
                    Convert.ToInt16(_context.Resources.DisplayMetrics.HeightPixels * 0.1),
                    Convert.ToInt16(_context.Resources.DisplayMetrics.HeightPixels * 0.1)
                );

                int current = index;
                imgView.Click += (sender, e) =>
                {
                    if (ImageClick != null)
                        ImageClick(this, new ImageCollectionClickEventArgs { Index = current });
                };

                imgView.SetScaleType(ImageView.ScaleType.CenterCrop);

                ImageCollectionHolder.AddView(imgView);

                LinearLayout imagePadding = new LinearLayout(_context);
                imagePadding.LayoutParameters = new ViewGroup.LayoutParams(PixelConverter.DpToPixels(5), LayoutParams.MatchParent);

                ImageCollectionHolder.AddView(imagePadding);

                index++;
            }
        }
    }

    public class ImageCollectionClickEventArgs : EventArgs
    {
        public int Index { get; set; }
    }
}

