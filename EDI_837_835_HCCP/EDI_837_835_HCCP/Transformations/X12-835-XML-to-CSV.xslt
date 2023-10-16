<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="text" indent="yes"/>
  <xsl:param name="filename" />


  <xsl:template match="Interchange">
    <xsl:text>File Name,Check Sequence,Payor,Payor Business Contact Name,Payor Business Contact,Payor Technical Contact Name,Payor Technical Contact,Payor Technical Email,Payor Technical Contact Url,Payee Name,Payee Identification Qualifier,Payee Identification,Claim Sequence Number,Claim Submitter Identifier,Claim Status Code,Total Claim Charge Amount,Total Claim Payment Amount,Patient Responsibility Amount,Payer Claim Control Number,POS,Patient LastName,Patient FirstName,Patient Identifier,Service Provider,Service Provider NPI,Service Code Qualifier,ServiceCode,ServiceCode Mod 01,ServiceCode Mod 02,ServiceCode Mod 03,ServiceCode Mod 04,Billed Amount,Paid Amount,Service Code Unit,Service StartDate,Service EndDate,Claim Adjustment Group Code,Claim Adjustment Reason Code,Claim Level Adjustment Amount, Reference Identification,AllowedAmount,CheckDate,CheckAmount,CheckNumber,PolicyNumber,AccountNumber,ICN,Deductible,Coins,Processed Date&#x0A;</xsl:text>
    <xsl:apply-templates select="FunctionGroup/Transaction"/>
  </xsl:template>

  <xsl:template match="Transaction">
    <xsl:variable name="sequence" select="position()"/>
    <xsl:for-each select="Loop[@LoopId='2000']/Loop[@LoopId='2100']/Loop[@LoopId='2110']">
      <xsl:call-template name="Procedure">
        <xsl:with-param name="checkSequence" select="$sequence"/>
        <xsl:with-param name="tran" select="../../../."/>
        <xsl:with-param name="payer" select="../../../Loop[@LoopId='1000A']"/>
        <xsl:with-param name="payee" select="../../../Loop[@LoopId='1000B']"/>
        <xsl:with-param name="header" select="../../."/>
        <xsl:with-param name="claim" select="../."/>
        <xsl:with-param name="service" select="."/>
      </xsl:call-template>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="Procedure">
    <xsl:param name="checkSequence"/>
    <xsl:param name="tran"/>
    <xsl:param name="payer"/>
    <xsl:param name="payee"/>
    <xsl:param name="header"/>
    <xsl:param name="claim"/>
    <xsl:param name="service"/>
    <!-- Filename -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$filename"/>
    <xsl:text>",</xsl:text>
    <!-- Check Sequence -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="'Check '"/>
    <xsl:value-of select="$checkSequence"/>
    <xsl:text>",</xsl:text>

    <!-- ///////////////////////////////////////// Payer Related Information ACCESS /////////////////////////////////////////////////////////// -->

    <!-- Payer Name = N102_PayorName-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/N1/N102"/>
    <xsl:text>",</xsl:text>

    <!-- Payer Business Contact Name = PER02_PayorBusinessContactName-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/PER[PER01='BL']/PER02"/>
    <xsl:text>",</xsl:text>

    <!-- Payer Business Contact  = PER04_PayorBusinessContact-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/PER[PER01='BL']/PER04"/>
    <xsl:text>",</xsl:text>



    <!-- Payer Technical Contact Name = PER02_PayorTechnicalContactName-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/PER[PER01='CX']/PER02"/>
    <xsl:text>",</xsl:text>

    <!-- Payer Technical Contact  = PER04_PayorTechnicalContact-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/PER[PER01='CX']/PER04"/>
    <xsl:text>",</xsl:text>


    <!-- Payer Technical Email = PER06_PayorTechnicalEmail-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/PER[PER01='BL']/PER06"/>
    <xsl:text>",</xsl:text>

    <!-- Payor Technical Contac tUrl  = PER04_PayorTechnicalContactUrl-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/PER[PER01='IC']/PER04"/>
    <xsl:text>",</xsl:text>





    <!-- ///////////////////////////////////////// Payee Related Information ACCESS /////////////////////////////////////////////////////////// -->

    <!-- Payee Name = N102_PayeeName -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payee/N1/N102"/>
    <xsl:text>",</xsl:text>
    <!-- Payee Identification Qualifier = N103_PayeeIdentificationQualifier-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payee/N1/N103"/>
    <xsl:text>",</xsl:text>
    <!-- Payee Identification = N104_PayeeIdentification-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payee/N1/N104"/>
    <xsl:text>",</xsl:text>


    <!-- ///////////////////////////////////////// Claim Related Information ACCESS /////////////////////////////////////////////////////////// -->

    <!-- Claim Sequence Number = LX01_ClaimSequenceNumber -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/LX/LX01"/>
    <xsl:text>",</xsl:text>


    <!-- Claim Submitter Identifier = CLP01_ClaimSubmitterIdentifier-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP01"/>
    <xsl:text>",</xsl:text>


    <!-- ClaimStatusCode = CLP02_ClaimStatusCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP02"/>
    <xsl:text>",</xsl:text>




    <!-- Total Claim Charge Amount = CLP03_TotalClaimChargeAmount-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP03"/>
    <xsl:text>",</xsl:text>

    <!-- Total Claim Paymet Amount = CLP04_TotalClaimPaymetAmount-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP04"/>
    <xsl:text>",</xsl:text>

    <!-- Patient Responsibility Amount = CLP05_PatientResponsibilityAmount-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP05"/>
    <xsl:text>",</xsl:text>

    <!-- Payer Claim Control Number = CLP07_PayerClaimControlNumber-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP07"/>
    <xsl:text>",</xsl:text>

    <!-- PlaceOfService = CLP08_PlaceOfService-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP08"/>
    <xsl:text>",</xsl:text>


    <!-- ///////////////////////////////////////// Patient Related Information ACCESS /////////////////////////////////////////////////////////// -->


    <!-- Patient Last Name = NM103_PatientLastName-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='QC']/NM103"/>
    <xsl:text>",</xsl:text>

    <!-- Patient First Name = NM104_PatientFirstName-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='QC']/NM104"/>
    <xsl:text>",</xsl:text>

    <!-- Patient Identifier = NM109_PatientIdentifier-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='QC']/NM109"/>
    <xsl:text>",</xsl:text>

    <!-- Service Provider Name = NM103_ServiceProviderName-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='82']/NM103"/>
    <xsl:text>",</xsl:text>

    <!-- Service Provider Npi = NM109_ServiceProviderNpi-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='82'  and NM108='XX']/NM109"/>
    <xsl:text>",</xsl:text>



    <!-- ///////////////////////////////////////// Service Code Related Information ACCESS /////////////////////////////////////////////////////////// -->

    <!-- ServiceCodeQualifier = SVC01_01_ServiceCodeQualifier-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0101"/>
    <xsl:text>",</xsl:text>

    <!-- ServiceCode = SVC01_02_ServiceCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0102"/>
    <xsl:text>",</xsl:text>




    <!-- ServiceCode = SVC01_02_ServiceCode_Mod_01-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0103"/>
    <xsl:text>",</xsl:text>


    <!-- ServiceCode = SVC01_02_ServiceCode_Mod_02-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0104"/>
    <xsl:text>",</xsl:text>


    <!-- ServiceCode = SVC01_02_ServiceCode_Mod_03-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0105"/>
    <xsl:text>",</xsl:text>


    <!-- ServiceCode = SVC01_02_ServiceCode_Mod_04-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0106"/>
    <xsl:text>",</xsl:text>




    <!-- SubmittedLineItemServiceChargeAmount = SVC02_SubmittedLineItemServiceChargeAmount-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC02"/>
    <xsl:text>",</xsl:text>

    <!-- SVC03_LineItemProviderPaymentAmoun_PA = SVC03_LineItemProviderPaymentAmoun_PA-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC03"/>
    <xsl:text>",</xsl:text>


    <!-- SVC05_ServiceCodeUnit = SVC05_ServiceCodeUnit-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC05"/>
    <xsl:text>",</xsl:text>

    <!-- ServiceStartDate = DTM02_ServiceStartDate-->
    <xsl:choose>
      <xsl:when test="string-length($service/DTM[DTM01='150']/DTM02) > 0">
        <xsl:call-template name="FormatDateYYYYMMDD">
          <xsl:with-param name="date" select="$service/DTM[DTM01='150']/DTM02"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="FormatDateYYYYMMDD">
          <xsl:with-param name="date" select="$service/DTM[DTM01='472']/DTM02"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text>,</xsl:text>


    <!-- ServiceEndDate = DTM02_ServiceEndDate-->
    <xsl:choose>
      <xsl:when test="string-length($service/DTM[DTM01='151']/DTM02) > 0">
        <xsl:call-template name="FormatDateYYYYMMDD">
          <xsl:with-param name="date" select="$service/DTM[DTM01='151']/DTM02"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="FormatDateYYYYMMDD">
          <xsl:with-param name="date" select="$service/DTM[DTM01='472']/DTM02"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text>,</xsl:text>

    <!-- CAS01_ClaimAdjustmentGroupCode = CAS01_ClaimAdjustmentGroupCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/CAS/CAS01"/>
    <xsl:text>",</xsl:text>

    <!-- CAS02_ClaimAdjustmentReasonCode = CAS02_ClaimAdjustmentReasonCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/CAS/CAS02"/>
    <xsl:text>",</xsl:text>

    <!-- CAS03_ClaimAdjustmentAmount = CAS03_ClaimAdjustmentAmount-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/CAS/CAS03"/>
    <xsl:text>",</xsl:text>


    <!-- ///////////////////////////////////////// LineItem Code Related Information ACCESS /////////////////////////////////////////////////////////// -->



    <!-- LineItem_ReferenceIdentification = REF02_LineItem_ReferenceIdentification-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/REF[REF01='6R']/REF02"/>
    <xsl:text>",</xsl:text>


    <!-- ServiceLineAllowedAmount = AMT01_ServiceLineAllowedAmount -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/AMT[AMT01='B6']/AMT02"/>
    <xsl:text>",</xsl:text>


    <!-- Check Date -->
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$tran/BPR/BPR16"/>
    </xsl:call-template>
    <xsl:text>,</xsl:text>
    <!-- Check Amount -->
    <xsl:value-of select="$tran/BPR/BPR02"/>
    <xsl:text>,</xsl:text>
    <!-- Check/EFT Number -->
    <xsl:value-of select="$tran/TRN/TRN02"/>
    <xsl:value-of select="','"/>

    <!-- Policy/HIC Number -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/REF[REF01='1L']/REF02"/>
    <xsl:text>",</xsl:text>
    <!-- Acct Number -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$tran/BPR/BPR09"/>
    <xsl:text>",</xsl:text>
    <!-- ICN -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/CLP/CLP07"/>
    <xsl:text>",</xsl:text>


    <!-- Billed Amount -->
    <!--<xsl:value-of select="$service/SVC/SVC02"/>
    <xsl:text>,</xsl:text>-->
    <!-- Allowed Amount -->
    <!--<xsl:value-of select="$service/AMT[AMT01='B6']/AMT02"/>
    <xsl:text>,</xsl:text>-->
    <!-- Deductible -->
    <xsl:value-of select="$service/AMT[AMT01='KH']/AMT02"/>
    <xsl:text>,</xsl:text>
    <!-- Coins -->
    <xsl:value-of select="$header/TS3/TS318"/>
    <xsl:text>,</xsl:text>
    <!-- Processed Date -->
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$tran/DTM/DTM02"/>
    </xsl:call-template>
    <xsl:text>&#x0A;</xsl:text>
    <!-- Paid Amount -->
    <!--<xsl:value-of select="$service/SVC/SVC03"/>
    <xsl:text>&#x0A;</xsl:text>-->
  </xsl:template>


  <xsl:template name="FormatDateYYYYMMDD">
    <xsl:param name="date"/>


    <xsl:choose>
      <xsl:when test="string-length($date) > 0">
        <xsl:value-of select="substring($date,5,2)"/>
        <xsl:text>/</xsl:text>
        <xsl:value-of select="substring($date,7,2)"/>
        <xsl:text>/</xsl:text>
        <xsl:value-of select="substring($date,1,4)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text></xsl:text>
      </xsl:otherwise>
    </xsl:choose>






    <!--<xsl:value-of select="substring($date,5,2)"/>
    <xsl:text>/</xsl:text>
    <xsl:value-of select="substring($date,7,2)"/>
    <xsl:text>/</xsl:text>
    <xsl:value-of select="substring($date,1,4)"/>-->
    
  </xsl:template>
    
  </xsl:stylesheet>