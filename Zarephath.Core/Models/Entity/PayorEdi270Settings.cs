using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using Zarephath.Core.Infrastructure.Attributes;

namespace Zarephath.Core.Models.Entity
{
    [TableName("PayorEdi270Settings")]
    [PrimaryKey("PayorEdi270SettingId")]
    [Sort("PayorEdi270SettingId", "DESC")]
    public class PayorEdi270Setting
    {
        public long PayorEdi270SettingId { get; set; }
        public string ISA01_AuthorizationInformationQualifier { get; set; }  // 00 :Common Value
        public string ISA02_AuthorizationInformation { get; set; } // 10 spaces :Default value
        public string ISA03_SecurityInformationQualifier { get; set; } //00 Common
        public string ISA04_SecurityInformation { get; set; } // 10 spaces :Default value
        public string ISA05_InterchangeSenderIdQualifier { get; set; } //@@
        public string ISA06_InterchangeSenderId { get; set; }  //@@@@@@@@@@@@@@@
        public string ISA07_InterchangeReceiverIdQualifier { get; set; } //@@
        public string ISA08_InterchangeReceiverId { get; set; } //@@@@@@@@@@@@@@@
        [Ignore]
        public string ISA09_InterchangeDate{get { return String.Format("{0:yyMMdd}", DateTime.Now); }} //Created Date  Format: YYMMDD
        [Ignore]
        public string ISA10_InterchangeTime{get { return String.Format("{0:hhmm}", DateTime.Now); }} //Created Time Format: HHMM

        public string ISA11_RepetitionSeparator { get; set; } // ^ :Common Value
        public string ISA12_InterchangeControlVersionNumber { get; set; } // 00501 :Default value
        public long ISA13_InterchangeControlNumber { get; set; } //00000001 :Begins with 00000001 and increments by +1 for each subsequent file create each day. Resets each day.
        public DateTime? ISA13_UpdatedDate { get; set; }
        public string ISA14_AcknowledgementRequired { get; set; } // 0 :Common Value
        public string ISA15_UsageIndicator { get; set; } //P  :Common Value
        public string ISA16_ComponentElementSeparator { get; set; } //@
        public string GS01_FunctionalIdentifierCode { get; set; } //HC :Default value
        public string GS02_ApplicationSenderCode { get; set; }  // @@@@@@@@@@@@@@@
        public string GS03_ApplicationReceiverCode { get; set; } // @@@@@@@@@@@@@@
        [Ignore]
        public string GS04_Date{get { return String.Format("{0:yyyyMMdd}", DateTime.Now); }} //Created Date  Format: YYYYMMDD
        [Ignore]
        public string GS05_Time { get { return String.Format("{0:hhmm}", DateTime.Now); } } //Created Time Format: HHMM
        public string GS06_GroupControlNumber { get; set; } // Begins with 1 and increments +1 for each subsequent GS within the file. Resets back to 1 with each new file.
        public string GS07_ResponsibleAgencyCode { get; set; } // X :Default value
        public string GS08_VersionOrReleaseOrNo { get; set; } // 005010X222A1
        public string ST01_TransactionSetIdentifier { get; set; } // 837 :Default value
        public string ST02_TransactionSetControlNumber { get; set; }
        public string ST03_ImplementationConventionReference { get; set; } // 005010X222A1 : Default Values
        public string BHT01_HierarchicalStructureCode { get; set; }
        public string BHT02_TransactionSetPurposeCode { get; set; }
        public string BHT03_ReferenceIdentification { get; set; }
        [Ignore]
        public string BHT04_Date { get { return String.Format("{0:yyyyMMdd}", DateTime.Now); }}
        [Ignore]
        public string BHT05_Time { get { return String.Format("{0:hhmmss}", DateTime.Now); } }
        public string BHT06_TransactionTypeCode { get; set; }

        public string SegmentTerminator { get; set; }
        public string ElementSeparator { get; set; }

    }
}
