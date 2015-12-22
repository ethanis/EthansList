using System;
using UIKit;
using Foundation;

namespace ethanslist.ios
{
    public class ConstraintHelper
    {
        public ConstraintHelper()
        {
        }

        //helper method to create constraints based on existing constraints
        public NSLayoutConstraint GetConstraint (
            NSLayoutConstraint constraint,
            NSObject object1 = null,
            NSLayoutAttribute? attribute1 = null,
            NSLayoutRelation? relation = null,
            NSObject object2 = null,
            NSLayoutAttribute? attribute2 = null,
            nfloat? multiplier = null,
            nfloat? constant = null)
        {
            if (constraint == null)
                return null;

            return NSLayoutConstraint.Create(
                object1 ?? constraint.FirstItem,
                (attribute1 == null) ? constraint.FirstAttribute : attribute1.Value,
                (relation == null) ? constraint.Relation : relation.Value,
                object2 ?? constraint.SecondItem, 
                (attribute2 == null) ? constraint.SecondAttribute : attribute2.Value,
                (multiplier == null) ? constraint.Multiplier : multiplier.Value,
                (constant == null) ? constraint.Constant : constant.Value);
        }
    }
}

