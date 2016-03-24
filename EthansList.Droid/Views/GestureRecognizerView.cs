
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace EthansList.Droid
{
    public class GestureRecognizerView : View, Koush.IUrlImageViewCallback
    {
        private static readonly int InvalidPointerId = -1;
        private ImageView imageView;

        private readonly Context _context;

        private Drawable _image;
        private ScaleGestureDetector _scaleDetector;

        private int _activePointerId = InvalidPointerId;
        private float _lastTouchX;
        private float _lastTouchY;
        private float _posX;
        private float _posY;
        public float _scaleFactor = 3.0f;

        public GestureRecognizerView(Context context, String imageUrl): base(context, null, 0)
        {
            _context = context;
            imageView = new ImageView(context);
            Koush.UrlImageViewHelper.SetUrlDrawable(imageView, imageUrl, Resource.Drawable.placeholder, this);
            _image = imageView.Drawable;
            _image.SetBounds(0, 0, _image.IntrinsicWidth, _image.IntrinsicHeight);

            //_scaleDetector = new ScaleGestureDetector(context, new MyScaleListener(this));

            //var metrics = Resources.DisplayMetrics;
            //_posX = GetCornerPosition(PixelConverter.PixelsToDp(metrics.WidthPixels), _image.Bounds.Width()) * (int)_scaleFactor;
            //_posY = GetCornerPosition(PixelConverter.PixelsToDp(metrics.HeightPixels), _image.Bounds.Height());
        }

        private int GetCornerPosition(int screenWidth, int imageWidth)
        {
            var padding = screenWidth - imageWidth;
            return padding / 2;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.Save();
            canvas.Translate(_posX, _posY);
            canvas.Scale(_scaleFactor, _scaleFactor);
            _image.Draw(canvas);
            canvas.Restore();
        }

        public override bool OnTouchEvent(MotionEvent ev)
        {
            _scaleDetector.OnTouchEvent(ev);

            MotionEventActions action = ev.Action & MotionEventActions.Mask;
            int pointerIndex;

            switch (action)
            {
                case MotionEventActions.Down:
                    _lastTouchX = ev.GetX();
                    _lastTouchY = ev.GetY();
                    _activePointerId = ev.GetPointerId(0);
                    break;

                case MotionEventActions.Move:
                    pointerIndex = ev.FindPointerIndex(_activePointerId);
                    float x = ev.GetX(pointerIndex);
                    float y = ev.GetY(pointerIndex);
                    if (!_scaleDetector.IsInProgress)
                    {
                        // Only move the ScaleGestureDetector isn't already processing a gesture.
                        float deltaX = x - _lastTouchX;
                        float deltaY = y - _lastTouchY;
                        _posX += deltaX;
                        _posY += deltaY;
                        Invalidate();
                    }

                    _lastTouchX = x;
                    _lastTouchY = y;
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    // We no longer need to keep track of the active pointer.
                    _activePointerId = InvalidPointerId;
                    break;

                case MotionEventActions.PointerUp:
                    // check to make sure that the pointer that went up is for the gesture we're tracking.
                    pointerIndex = (int) (ev.Action & MotionEventActions.PointerIndexMask) >> (int) MotionEventActions.PointerIndexShift;
                    int pointerId = ev.GetPointerId(pointerIndex);
                    if (pointerId == _activePointerId)
                    {
                        // This was our active pointer going up. Choose a new
                        // action pointer and adjust accordingly
                        int newPointerIndex = pointerIndex == 0 ? 1 : 0;
                        _lastTouchX = ev.GetX(newPointerIndex);
                        _lastTouchY = ev.GetY(newPointerIndex);
                        _activePointerId = ev.GetPointerId(newPointerIndex);
                    }
                    break;

            }
            return true;
        }

        public void OnLoaded(ImageView p0, Bitmap p1, string p2, bool p3)
        {
            _image = imageView.Drawable;
            _image.SetBounds(0, 0, _image.IntrinsicWidth, _image.IntrinsicHeight);
            _scaleDetector = new ScaleGestureDetector(_context, new MyScaleListener(this));

            var metrics = Resources.DisplayMetrics;
            _posX = GetCornerPosition(PixelConverter.PixelsToDp(metrics.WidthPixels), _image.Bounds.Width()) * (int)_scaleFactor;
            _posY = GetCornerPosition(PixelConverter.PixelsToDp(metrics.HeightPixels), _image.Bounds.Height());
        }
    }
}

