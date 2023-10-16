using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OopFactory.X12.Parsing.Model.Typed
{
    public class TypedLoopSBR : TypedLoop
    {
        public TypedLoopSBR()
            : base("SBR")
        {
        }

        public string SBR01_PayerResponsibilitySequenceNumberCode
        {
            get { return _loop.GetElement(1); }
            set { _loop.SetElement(1, value); }
        }

        public string SBR02_IndividualRelationshipCode
        {
            get { return _loop.GetElement(2); }
            set { _loop.SetElement(2, value); }
        }

        public string SBR03_PolicyOrGroupNumber
        {
            get { return _loop.GetElement(3); }
            set { _loop.SetElement(3, value); }
        }

        public string SBR04_GroupName
        {
            get { return _loop.GetElement(4); }
            set { _loop.SetElement(4, value); }
        }

        public string SBR05_InsuranceTypeCode
        {
            get { return _loop.GetElement(5); }
            set { _loop.SetElement(5, value); }
        }

        public string SBR06_CoordinationOfBenefitsCode
        {
            get { return _loop.GetElement(6); }
            set { _loop.SetElement(6, value); }
        }

        public string SBR07_YesNoCode
        {
            get { return _loop.GetElement(7); }
            set { _loop.SetElement(7, value); }
        }

        public string SBR08_EmploymentStatusCode
        {
            get { return _loop.GetElement(8); }
            set { _loop.SetElement(8, value); }
        }

        public string SBR09_ClaimFilingIndicatorCode
        {
            get { return _loop.GetElement(9); }
            set { _loop.SetElement(9, value); }
        }
    }



    public class TypedLoopSVD : TypedLoop
    {
        public TypedLoopSVD()
            : base("SVD")
        {
        }

        public string SVD01_OtherPayerPrimaryIdentifier
        {
            get { return _loop.GetElement(1); }
            set { _loop.SetElement(1, value); }
        }

        public string SVD02_MonetaryAmount
        {
            get { return _loop.GetElement(2); }
            set { _loop.SetElement(2, value); }
        }

        public string SVD03_ProductOrServiceIDComposite
        {
            get { return _loop.GetElement(3); }
            set { _loop.SetElement(3, value); }
        }

        public string SVD05_PaidServiceUnitCount
        {
            get { return _loop.GetElement(5); }
            set { _loop.SetElement(5, value); }
        }

        
    }


    
    public class TypedSegmentCAS : TypedSegment
    {
        public TypedSegmentCAS()
            : base("CAS")
        {
        }

        public string CAS01_ClaimAdjustmentGroupCode
        {
            get { return _segment.GetElement(1); }
            set { _segment.SetElement(1, value); }
        }

        public string CAS02_ClaimAdjustmentReasonCode
        {
            get { return _segment.GetElement(2); }
            set { _segment.SetElement(2, value); }
        }

        public string CAS03_MonetaryAmount
        {
            get { return _segment.GetElement(3); }
            set { _segment.SetElement(3, value); }
        }

        
    }


    
}
