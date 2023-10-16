<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
                xmlns:ext277="http://XsltSampleSite.XsltFunctions/1.0"  >

  <xsl:output method="text" indent="yes"/>
  <xsl:param name="filename" />

  <xsl:template match="Interchange">
    <xsl:text>Source,Receipt Date,Process Date,Receiver,Receiver LVL - Status,Receiver LVL - CSCC|CSC|EIC,Receiver LVL - Action,Receiver LVL - Message,Provider,Provider LVL - Status,Provider LVL - CSCC|CSC|EIC,Provider LVL - Action,Provider LVL - Message,Patient,AHCCCS #,Patient LVL - Status,Provider - CSCC|CSC|EIC,Patient LVL - Action,Patient LVL - Amount,Patient LVL - Message,Batch #, Note #, Claim #, Payor Claim#, Service Date&#x0A;</xsl:text>
    <xsl:apply-templates select="FunctionGroup"/>
  </xsl:template>

  <xsl:template match="FunctionGroup">
    <!--<xsl:variable name="sequence" select="position()"/>-->
    <!--<xsl:for-each select="Transaction/HierarchicalLoop[@LoopId='2000A']/HierarchicalLoop[@LoopId='2000B']/HierarchicalLoop[@LoopId='2000C']/HierarchicalLoop[@LoopId='2000D']">-->
    <xsl:for-each select="Transaction">
      <xsl:call-template name="Procedure">
        <xsl:with-param name="source" select="./HierarchicalLoop[@LoopId='2000A']"/>
        <xsl:with-param name="receiver" select="./HierarchicalLoop[@LoopId='2000A']/HierarchicalLoop[@LoopId='2000B']"/>
        <xsl:with-param name="provider" select="./HierarchicalLoop[@LoopId='2000A']/HierarchicalLoop[@LoopId='2000B']/HierarchicalLoop[@LoopId='2000C']"/>
        <xsl:with-param name="patient" select="./HierarchicalLoop[@LoopId='2000A']/HierarchicalLoop[@LoopId='2000B']/HierarchicalLoop[@LoopId='2000C']/HierarchicalLoop[@LoopId='2000D']"/>



      </xsl:call-template>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="Procedure">
    <xsl:param name="source"/>
    <xsl:param name="receiver"/>
    <xsl:param name="provider"/>
    <xsl:param name="patient"/>


    <!-- Source -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($source/Loop[@LoopId='2100A']/NM1/NM103,' - ', $source/Loop[@LoopId='2100A']/NM1/NM109)"/>
    <xsl:text>",</xsl:text>

    <!-- Receipt Date -->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$source/Loop[@LoopId='2200A']/DTP[DTP01='050']/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>

    <!-- Process Date -->
    <xsl:text>"</xsl:text>
    <xsl:call-template name="FormatDateYYYYMMDD">
      <xsl:with-param name="date" select="$source/Loop[@LoopId='2200A']/DTP[DTP01='009']/DTP03"/>
    </xsl:call-template>
    <xsl:text>",</xsl:text>
    
    <!-- Receiver -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($receiver/Loop[@LoopId='2100B']/NM1/NM103,' - ', $receiver/Loop[@LoopId='2100B']/NM1/NM109)"/>
    <xsl:text>",</xsl:text>

    <!-- Receiver LVL - Status -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimStatusCategoryCodes($receiver/Loop[@LoopId='2200B']/STC/STC01/STC0101)"/>
    <xsl:text>",</xsl:text>


    <!-- Receiver LVL - CSCC+CSC+EIC-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($receiver/Loop[@LoopId='2200B']/STC/STC01/STC0101,' | ', $receiver/Loop[@LoopId='2200B']/STC/STC01/STC0102,' | ', $receiver/Loop[@LoopId='2200B']/STC/STC01/STC0103)"/>
    <!--<xsl:value-of select="$receiver/Loop[@LoopId='2200B']/STC/STC01"/>-->
    <xsl:text>",</xsl:text>


    <!-- Receiver LVL - Action -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimStatus($receiver/Loop[@LoopId='2200B']/STC/STC03)"/>
    <xsl:text>",</xsl:text>


    <!-- Receiver LVL - Message-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$receiver/Loop[@LoopId='2200B']/STC/STC12"/>
    <xsl:text>",</xsl:text>



    <!-- Provider -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($provider/Loop[@LoopId='2100C']/NM1/NM103,' - ', $provider/Loop[@LoopId='2100C']/NM1/NM109)"/>
    <xsl:text>",</xsl:text>

    <!-- Provider LVL - Status -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimStatusCategoryCodes($provider/Loop[@LoopId='2200C']/STC/STC01/STC0101)"/>
    <xsl:text>",</xsl:text>


    <!-- Provider LVL - CSCC+CSC+EIC-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($provider/Loop[@LoopId='2200C']/STC/STC01/STC0101,' | ', $provider/Loop[@LoopId='2200C']/STC/STC01/STC0102,' | ', $provider/Loop[@LoopId='2200C']/STC/STC01/STC0103)"/>
    <!--<xsl:value-of select="$provider/Loop[@LoopId='2200C']/STC/STC01"/>-->
    <xsl:text>",</xsl:text>


    <!-- Provider LVL - Action-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimStatus($provider/Loop[@LoopId='2200C']/STC/STC03)"/>
    <xsl:text>",</xsl:text>

    <!-- Provider LVL - Message-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$provider/Loop[@LoopId='2200C']/STC/STC12"/>
    <xsl:text>",</xsl:text>



    <!-- Patient -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="concat($patient/Loop[@LoopId='2100D']/NM1/NM103,', ', $patient/Loop[@LoopId='2100D']/NM1/NM104)"/>
    <xsl:text>",</xsl:text>


    <!-- AHCCCS #-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$patient/Loop[@LoopId='2100D']/NM1/NM109"/>
    <xsl:text>",</xsl:text>

    <!-- Patient LVL - Status-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimStatusCategoryCodes($patient/Loop[@LoopId='2200D']/STC/STC01/STC0101)"/>
    <xsl:text>",</xsl:text>

    <!-- Patient LVL - CSCC+CSC+EIC-->
    <xsl:text>"</xsl:text>
    <!--<xsl:value-of select="$patient/Loop[@LoopId='2200D']/STC/STC01"/>-->
    <xsl:value-of select="concat($patient/Loop[@LoopId='2200D']/STC/STC01/STC0101,' | ', $patient/Loop[@LoopId='2200D']/STC/STC01/STC0102,' | ', $patient/Loop[@LoopId='2200D']/STC/STC01/STC0103)"/>
    <xsl:text>",</xsl:text>



    <!-- Patient LVL - Action-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimStatus($patient/Loop[@LoopId='2200D']/STC/STC03)"/>
    <xsl:text>",</xsl:text>

    <!-- Patient LVL - Amount-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$patient/Loop[@LoopId='2200D']/STC/STC04"/>
    <xsl:text>",</xsl:text>

    <!-- Patient LVL - Message-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$patient/Loop[@LoopId='2200D']/STC/STC12"/>
    <xsl:text>",</xsl:text>


    
    <!-- Patient LVL - Batch #-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimDetails($patient/Loop[@LoopId='2200D']/TRN/TRN02,'Batch')"/>
    <xsl:text>",</xsl:text>


    <!-- Patient LVL - Note #-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="ext277:GetClaimDetails($patient/Loop[@LoopId='2200D']/TRN/TRN02,'Note')"/>
    <xsl:text>",</xsl:text>




    <!-- Claim Number-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$patient/Loop[@LoopId='2200D']/TRN/TRN02"/>
    <xsl:text>",</xsl:text>

    <!-- Patient LVL - Payor's Claim Number-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$patient/Loop[@LoopId='2200D']/REF/REF02"/>
    <xsl:text>",</xsl:text>

    <!-- Patient LVL - ServiceDate-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$patient/Loop[@LoopId='2200D']/DTP/DTP03"/>
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