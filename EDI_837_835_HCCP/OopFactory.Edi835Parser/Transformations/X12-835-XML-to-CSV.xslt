<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
  <xsl:output method="text" indent="yes"/>
  <xsl:param name="filename" />
  

  <xsl:template match="Interchange">
    <xsl:text>Filename, Check Sequence, Payer Name,Payee Name,Payee ID,check Date,Check $,Check/EFT Number,NPI,Patient Name,Policy/HIC Number,Acct Number,ICN,Date Of Ser From,Date Of Ser To,Procedure,Billed Amount,Allowed Amount,Deductible,Coins,Paid Amount&#x0A;</xsl:text>
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
    <!-- Payer Name-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payer/N1/N102"/>
    <xsl:text>",</xsl:text>
    <!-- Payee Name-->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payee/N1/N102"/>
    <xsl:text>",</xsl:text>
    <!-- Payee ID -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$payee/N1/N104"/>
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
    <!-- NPI -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='82' and NM108='XX']/NM109"/>
    <xsl:text>",</xsl:text>
    <!-- Patient Name -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='QC']/NM103"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="$claim/NM1[NM101='QC']/NM104"/>
    <xsl:text>",</xsl:text>
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
    <!-- Date of Service From -->
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
    <!-- Date of Service To -->
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
    <!-- Procedure -->
    <xsl:text>"</xsl:text>
    <xsl:value-of select="$service/SVC/SVC01/SVC0102"/>
    <xsl:text>",</xsl:text>
    <!-- Billed Amount -->
    <xsl:value-of select="$service/SVC/SVC02"/>
    <xsl:text>,</xsl:text>
    <!-- Allowed Amount -->
    <xsl:value-of select="$service/AMT[AMT01='B6']/AMT02"/>
    <xsl:text>,</xsl:text>
    <!-- Deductible -->
    <xsl:value-of select="$service/AMT[AMT01='KH']/AMT02"/>
    <xsl:text>,</xsl:text>
    <!-- Coins -->
    <xsl:value-of select="$header/TS3/TS318"/>
    <xsl:text>,</xsl:text>
    <!-- Paid Amount -->
    <xsl:value-of select="$service/SVC/SVC03"/>
    <xsl:text>&#x0A;</xsl:text>
  </xsl:template>

  <xsl:template name="FormatDateYYYYMMDD">
    <xsl:param name="date"/>
    <xsl:value-of select="substring($date,5,2)"/>
    <xsl:text>/</xsl:text>
    <xsl:value-of select="substring($date,7,2)"/>
    <xsl:text>/</xsl:text>
    <xsl:value-of select="substring($date,1,4)"/>
  </xsl:template>
</xsl:stylesheet>