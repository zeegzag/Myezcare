<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:ext="http://XsltSampleSite.XsltFunctions/1.0"  >
  
  <xsl:output method="text" indent="yes"/>
  <xsl:param name="filename" />

  <xsl:template match="Interchange">
    <xsl:text>Source,Receiver,Last Name,First Name,AHCCCS ID,Gender,Dob,Address,City,State,Zipcode,Client Response Code,Client Reject Reason Code,Client Follow Up Action,Eligible?,Eligibility - Insurance Type,Eligibility - Group, Eligibility - Date, ENROLL FFS - Service Type,ENROLL FFS - Insurance Type,ENROLL FFS - Group,ENROLL FFS - Date,ENROLL Capitation - Service Type,ENROLL Capitation - Insurance Type,ENROLL Capitation - Group,ENROLL - Capitation Date,COPAY - Plan, COPAY - Date,MEDICARE HMO - Service Type,MEDICARE HMO - Plan,MEDICARE HMO - Date,MEDICARE PART A - Service Type,MEDICARE PART A - Insurance Type,MEDICARE PART A - Date,MEDICARE PART B - Service Type,MEDICARE PART B - Insurance Type,MEDICARE PART B - Date,MEDICARE PART D - Service Type,MEDICARE PART D - Insurance Type,MEDICARE PART D - Plan,MEDICARE PART D - Date,TPL - Service Type,TPL - Insurance Type,TPL - Plan,TPL - Date,BHS - Service Type,BHS - Plan,BHS - Date,CRS - Service Type,CRS - Plan, CRS - Date,TSC - Service Type,TSC - Plan,TSC - Date,AZEIP - Service Type,AZEIP - Plan,AZEIP - Date,SHARE OF COST - Benefit Amt,SHARE OF COST - Benefit Date,Source Response Code,Source Reject Reason Code,Source Follow Up Action,Receiver Response Code,Receiver Reject Reason Code,Receiver Follow Up Action &#x0A;</xsl:text>
    <xsl:apply-templates select="FunctionGroup/Transaction"/>
  </xsl:template>

  <xsl:template match="Transaction">
    <!--<xsl:variable name="sequence" select="position()"/>-->
    <xsl:for-each select="HierarchicalLoop[@LoopId='2000A']/HierarchicalLoop[@LoopId='2000B']/HierarchicalLoop[@LoopId='2000C']/Loop[@LoopId='2100C']">
      <xsl:call-template name="Procedure">
        <xsl:with-param name="source" select="../../../Loop[@LoopId='2100A']"/>
        <xsl:with-param name="receiver" select="../../Loop[@LoopId='2100B']"/>
        <xsl:with-param name="client" select="."/>
        <xsl:with-param name="eligibility" select="./Loop[@LoopId='2110C']"/>
      </xsl:call-template>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="Procedure">
    <xsl:param name="source"/>
    <xsl:param name="receiver"/>
    <xsl:param name="client"/>
    <xsl:param name="eligibility"/>
    

    <!-- Filename --><!--
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$filename"/>
    <xsl:text>",</xsl:text>-->

    <!-- Source -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($source/NM1/NM103,' - ', $source/NM1/NM109)"/>
    <xsl:text>",</xsl:text>

    <!-- Source Contact Informations--><!--
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$source/PER/PER04"/>
    <xsl:text>",</xsl:text>-->

    
    

    <!-- Receiver -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($receiver/NM1/NM103,' - ', $receiver/NM1/NM109)"/>
    <xsl:text>",</xsl:text>


    
    <!-- Last Name-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/NM1/NM103"/>
    <xsl:text>",</xsl:text>

    <!-- First Name-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/NM1/NM104"/>
    <xsl:text>",</xsl:text>


    <!-- AHCCCS NUMBER-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/NM1/NM109"/>
    <xsl:text>",</xsl:text>


    <!-- Gender-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/DMG/DMG03"/>
    <xsl:text>",</xsl:text>


    <!-- DOB-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$client/DMG/DMG02"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>


    <!-- Address -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/N3/N301"/>
    <xsl:text>",</xsl:text>

    <!-- City-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/N4/N401"/>
    <xsl:text>",</xsl:text>

    <!-- State-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/N4/N402"/>
    <xsl:text>",</xsl:text>


    <!-- ZipCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/N4/N403"/>
    <xsl:text>",</xsl:text>

    <!--Client_ResponseCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$client/AAA/AAA01"/>
    <xsl:text>",</xsl:text>

    <!--Client_RejectReasonCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetRejectResponseFromCode($client/AAA/AAA03)"/>
    <xsl:text>",</xsl:text>

    <!--Client_FollowUpActionCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetFollowUpActionCode($client/AAA/AAA04)"/>
    <xsl:text>",</xsl:text>


    <!-- Eligibile ? -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetEligibilityStatus($eligibility/EB[EB01='6' or EB01='7' or EB01='8']/EB01)"/>
    <xsl:text>",</xsl:text>

    <!-- Eligibility Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetInsuranceTypeFromCode($eligibility/EB[EB01='1' and EB03='']/EB04)"/>
    <xsl:text>",</xsl:text>
    
    <!-- Eligibility Group -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='1' and EB03='']/EB05"/>
    <xsl:text>",</xsl:text>

    <!-- Eligibility Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility/DTP[DTP01='307']/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>


    <!-- ENROLLFFS  Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='1' and EB03!='' and EB04='MC']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- ENROLLFFS  Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetInsuranceTypeFromCode($eligibility/EB[EB01='1' and EB03!='' and EB04='MC']/EB04)"/>
    <xsl:text>",</xsl:text>

    <!-- ENROLLFFS Group -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='1' and EB03!='' and EB04='MC']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- ENROLLFFS  Group  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='1' and EB03!='' and EB04='MC']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>


    

    <!-- ENROLL Capitation  Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='3' and EB03='30' and EB04='HM']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- ENROLL Capitation  Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetInsuranceTypeFromCode($eligibility/EB[EB01='3' and EB03='30' and EB04='HM']/EB04)"/>
    <xsl:text>",</xsl:text>

    <!-- ENROLL Capitation Group -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='3' and EB03='30' and EB04='HM']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- ENROLL Capitation Group  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='3' and EB03='30' and EB04='HM']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>


    <!-- CoPay  Plan -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='D' and EB05='COPAY LEVEL']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- CoPay   Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='D' and EB05='COPAY LEVEL']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>




    <!-- MEDICARE HMO  Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='' and EB05='MEDICARE HMO']/EB03)"/>
    <xsl:text>",</xsl:text>


    <!-- MEDICARE HMO Group -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='R' and EB03='30' and EB04='' and EB05='MEDICARE HMO']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- MEDICARE HMO  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='R' and EB03='30' and EB04='' and EB05='MEDICARE HMO']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>






    <!-- MEDICARE Part A Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='MA']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- MEDICARE Part A Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetInsuranceTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='MA']/EB04)"/>
    <xsl:text>",</xsl:text>


    <!-- MEDICARE Part A  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='R' and EB03='30' and EB04='MA']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>





    <!-- MEDICARE Part B Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='MB']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- MEDICARE Part B Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetInsuranceTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='MB']/EB04)"/>
    <xsl:text>",</xsl:text>

  

    <!-- MEDICARE Part B  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='R' and EB03='30' and EB04='MB']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>



    <!-- MEDICARE PART D  Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='R' and EB03='88' and EB04='OT']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- MEDICARE PART D  Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetInsuranceTypeFromCode($eligibility/EB[EB01='R' and EB03='88' and EB04='OT']/EB04)"/>
    <xsl:text>",</xsl:text>

    <!-- MEDICARE PART D Plan -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='R' and EB03='88' and EB04='OT']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- MEDICARE PART D  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='R' and EB03='88' and EB04='OT']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>






    <!-- TPL Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='C1']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- TPL Insurance Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='R' and EB03='30' and EB04='C1']/EB04)"/>
    <xsl:text>",</xsl:text>

    <!-- TPL  Plan -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='R' and EB03='30' and EB04='C1']/EB05"/>
    <xsl:text>",</xsl:text>

    <!-- TPL Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='R' and EB03='30' and EB04='C1']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>


    <!-- BHS Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='3' and EB03='CH']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- BHS  Plan -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='3' and EB03='CH']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- BHS Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='3' and EB03='CH']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>



    <!-- CRS Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='3' and EB03='A9']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- CRS  Plan -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='3' and EB03='A9']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- CRS  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='3' and EB03='A9']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>




    <!-- TSC Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='3' and EB03='CQ']/EB03)"/>
    <xsl:text>",</xsl:text>

    <!-- TSC  Plan -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='3' and EB03='CQ']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- TSC  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='3' and EB03='CQ']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>




    <!-- AZEIP Service Type -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetServiceCodeTypeFromCode($eligibility/EB[EB01='3' and EB03='CQ' and EB05='AZ EARLY INTERVENTI ON PROGRAM']/EB03)"/>
    <xsl:text>",</xsl:text>


    <!-- AZEIP  Plan  -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='3' and EB03='CQ'  and EB05='AZ EARLY INTERVENTI ON PROGRAM']/EB05"/>
    <xsl:text>",</xsl:text>


    <!-- AZEIP  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='3' and EB03='CQ'  and EB05='AZ EARLY INTERVENTI ON PROGRAM']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>

    
    
    
    <!-- SHARE OF COST  Benefit Amout -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$eligibility/EB[EB01='G']/EB07"/>
    <xsl:text>",</xsl:text>


    <!-- SHARE OF COST  Benefit  Date-->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$eligibility[EB[EB01='G']]/DTP/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>


    <!--Source_ResponseCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$source/AAA/AAA01"/>
    <xsl:text>",</xsl:text>

    <!--Source_RejectReasonCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetRejectResponseFromCode($source/AAA/AAA03)"/>
    <xsl:text>",</xsl:text>

    <!--Source_FollowUpActionCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetFollowUpActionCode($source/AAA/AAA04)"/>
    <xsl:text>",</xsl:text>

    <!--Receiver_ResponseCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$receiver/AAA/AAA01"/>
    <xsl:text>",</xsl:text>

    <!--Receiver_RejectReasonCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetRejectResponseFromCode($receiver/AAA/AAA03)"/>
    <xsl:text>",</xsl:text>

    <!--Receiver_FollowUpActionCode-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext:GetFollowUpActionCode($receiver/AAA/AAA04)"/>
    <xsl:text>"</xsl:text>



    <xsl:text>&#x0A;</xsl:text>



  </xsl:template>






  <xsl:template name="FormatDateYYYYMMDD">
    <xsl:param name="date"/>
    <xsl:if test="$date">
      <xsl:value-of select="substring($date,5,2)"/>
      <xsl:text>/</xsl:text>
      <xsl:value-of select="substring($date,7,2)"/>
      <xsl:text>/</xsl:text>
      <xsl:value-of select="substring($date,1,4)"/>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>