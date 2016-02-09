using System;
using UIKit;
using System.Collections.Generic;
using System.Drawing;

namespace ethanslist.ios
{
    public class SearchPickerModel : UIPickerViewModel
    {
        public List<PickerOptions> values;
        private readonly bool price;
        private const string cellID = "cellID";

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public SearchPickerModel(List<PickerOptions> values, bool price)
        {
            this.values = values;
            this.price = price;
        }

        public override nint GetComponentCount (UIPickerView picker)
        {
            return values.Count;
        }

        public override nint GetRowsInComponent (UIPickerView picker, nint component)
        {
            if (price)
                return values[(int)component].Options.Count;
            else
                return values[(int)component].PickerWheelOptions.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (price)
                return values[(int)component].Options[(int)row].ToString ();
            else
                return values[(int)component].PickerWheelOptions[(int)row].Key.ToString ();
        }

        public override void Selected (UIPickerView picker, nint row, nint component)
        {
            if (this.PickerChanged != null)
            {
                if (price)
                    this.PickerChanged(this, new PickerChangedEventArgs{SelectedValue = values[(int)component].Options[(int)row], FromComponent = (int)component});
                else
                    this.PickerChanged(this, new PickerChangedEventArgs
                        {SelectedValue = values[(int)component].PickerWheelOptions[(int)row].Value, 
                            SelectedKey = values[(int)component].PickerWheelOptions[(int)row].Key, 
                            FromComponent = (int)component
                        });
            }
        }
    }

    public class PickerChangedEventArgs : EventArgs{
        public object SelectedValue {get;set;}
        public object SelectedKey { get; set; }
        public int FromComponent {get;set;}
    }

    public class PopupTableView : UIView
    {
        public enum PopUpSizeProportion
        {
            Full,
            Half,
            TwoThird,
        };

        private float GetPopupSizeProportionValue(PopUpSizeProportion proportion)
        {
            switch (proportion)
            {
                case PopUpSizeProportion.Half:
                    return 0.5f;
                case PopUpSizeProportion.TwoThird:
                    return 0.75f;
                case PopUpSizeProportion.Full:
                    return 1.0f;
                default:
                    return 0.5f;
            }
        }

        public UITableView Table { get; set; }
        public UILabel TitleLabel { get; set; }
        public UIView Overlay { get; set; }
        public UIButton Button { get; set; }
        public UIView ContentView { get; set; }

        CoreGraphics.CGRect _frame;
        PopUpSizeProportion _proportion;


        public PopupTableView (CoreGraphics.CGRect frame, PopUpSizeProportion proportion)
            :base(frame)
        {
            _frame = frame;
            _proportion = proportion;

            Table = new UITableView();
            TitleLabel = new UILabel();
            Overlay = new UIView();
            Button = new UIButton();
            ContentView = new UIView();
        }

        public void Show(UIViewController parent)
        {
            Alpha = 0.0f;

            nfloat centerX = _frame.Width / 2;
            nfloat centerY = _frame.Height / 2;

            var margin = 15.0f;

            var popupHeight = (_proportion == PopUpSizeProportion.Full) ?
                (_frame.Height * GetPopupSizeProportionValue(_proportion)) - (margin * 2 + parent.NavigationController.NavigationBar.Bounds.Height + 5.0f)
                : _frame.Height * GetPopupSizeProportionValue(_proportion);

            var popupWidth = (_proportion == PopUpSizeProportion.Full) ? (_frame.Width * GetPopupSizeProportionValue(_proportion)) - margin : _frame.Width * GetPopupSizeProportionValue(_proportion);

            var popupCenterX = popupWidth / 2;

            var contentContainerY = (_proportion == PopUpSizeProportion.Full) ? parent.NavigationController.NavigationBar.Bounds.Height + margin + 5.0f : centerY - (popupHeight / 2);

            var buttonWidth = 140.0f;
            var buttonHeight = 60.0f;

            var titleHeight = 100;
            var titleWidth = popupWidth - (margin * 2);

            var tableHeight = popupHeight - (( titleHeight ) + ( margin * 2 ) + ( buttonHeight ) + ( margin * 2 ));
            var tableY = titleHeight + (margin * 2);                

            TitleLabel.Frame = new CoreGraphics.CGRect(margin, margin, titleWidth, titleHeight);
            Table.Frame = new CoreGraphics.CGRect(margin, tableY, (float)titleWidth, (float)tableHeight);
            Button.Frame = new CoreGraphics.CGRect(popupCenterX - (buttonWidth / 2), (float)(popupHeight - (buttonHeight + margin)), (float)buttonWidth, (float)buttonHeight);
            ContentView.Frame = new CoreGraphics.CGRect(centerX - (popupWidth / 2), (float)contentContainerY, (float)popupWidth, (float)popupHeight);

            Overlay.Frame = _frame;

            ContentView.AddSubview(TitleLabel);
            ContentView.AddSubview(Table);
            ContentView.AddSubview(Button);

            AddSubview(Overlay);
            AddSubview(ContentView);

            parent.Add(this);

            Animate(
                0.3f,
                () => Alpha = 1.0f);

        }

        public void Hide()
        {

            UIView.Animate(
                0.3f,
                () => { Alpha = 0; },
                () => RemoveFromSuperview());

            this.Dispose();
        }
    }
}

