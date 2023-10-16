using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OopFactory.X12.Parsing.Model.Typed
{
    public class TypedSegmentSV2 : TypedSegment
    {
        public TypedSegmentSV2() : base("SV2")
        {
        }

        public string SV201_Product_ServiceID
        {
            get { return _segment.GetElement(1); }
            set { _segment.SetElement(1, value); }
        }

        public string SV202_CompositeMedicalProcedure
        {
            get { return _segment.GetElement(2); }
            set { _segment.SetElement(2, value); }
        }

        public string SV203_MonetaryAmount
        {
            get { return _segment.GetElement(3); }
            set { _segment.SetElement(3, value); }
        }

        public string SV204_UnitBasisMeasCode
        {
            get { return _segment.GetElement(4); }
            set { _segment.SetElement(4, value); }
        }

        public string SV205_Quantity
        {
            get { return _segment.GetElement(5); }
            set { _segment.SetElement(5, value); }
        }
    }
}
