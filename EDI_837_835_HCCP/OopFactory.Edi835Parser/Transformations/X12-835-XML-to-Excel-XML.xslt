<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
      xmlns="urn:schemas-microsoft-com:office:spreadsheet"
        xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
     
                >
  <xsl:output method="xml" indent="yes"/>
  <xsl:param name="filename" />


  <xsl:template match="Interchange">
    <xsl:processing-instruction name="mso-application">progid="Excel.Sheet"</xsl:processing-instruction>
    <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
     xmlns:o="urn:schemas-microsoft-com:office:office"
     xmlns:x="urn:schemas-microsoft-com:office:excel"
     xmlns:html="http://www.w3.org/TR/REC-html40">
      <DocumentProperties xmlns="urn:schemas-microsoft-com:office:office">
        <Author>Dannie Strubhar</Author>
        <LastAuthor>Dannie Strubhar</LastAuthor>
        <Created>2011-10-04T19:23:22Z</Created>
        <Version>14.00</Version>
      </DocumentProperties>
      <OfficeDocumentSettings xmlns="urn:schemas-microsoft-com:office:office">
        <AllowPNG/>
      </OfficeDocumentSettings>
      <ExcelWorkbook xmlns="urn:schemas-microsoft-com:office:excel">
        <WindowHeight>5445</WindowHeight>
        <WindowWidth>14355</WindowWidth>
        <WindowTopX>480</WindowTopX>
        <WindowTopY>90</WindowTopY>
        <ProtectStructure>False</ProtectStructure>
        <ProtectWindows>False</ProtectWindows>
      </ExcelWorkbook>
      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#000000"/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="s62">
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#000000"
           ss:Bold="1"/>
        </Style>
        <Style ss:ID="s64">
          <Borders>
            <Border ss:Position="Top" ss:LineStyle="Double" ss:Weight="3"/>
          </Borders>
        </Style>
        <Style ss:ID="s68">
          <Interior ss:Color="#EBF1DE" ss:Pattern="Solid"/>
        </Style>
        
      </Styles>
      <Worksheet>
        <xsl:attribute name="ss:Name">
          <xsl:value-of select="$filename"/>
        </xsl:attribute>
        <Table ss:ExpandedColumnCount="21" x:FullColumns="1"
         ss:DefaultRowHeight="15">
          <Column ss:Width="100"/>
          <Column ss:Width="84"/>
          <Column ss:Width="100"/>
          <Column ss:Width="160"/>
          <Column ss:Width="60"/>
          <Column ss:Width="56"/>
          <Column ss:Width="48"/>
          <Column ss:Width="96"/>
          <Column ss:Width="60"/>
          <Column ss:Width="128"/>
          <Column ss:Width="96"/>
          <Column ss:Width="68"/>
          <Column ss:Width="84"/>
          <Column ss:Width="104"/>
          <Column ss:Width="92"/>
          <Column ss:Width="72"/>
          <Column ss:Width="88"/>
          <Column ss:Width="88"/>
          <Column ss:Width="88"/>
          <Column ss:Width="88"/>
          <Column ss:Width="88"/>
          <Row ss:AutoFitHeight="0" ss:StyleID="s62">
            <Cell>
              <Data ss:Type="String">Filename</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String"> Check Sequence</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String"> Payer Name</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Payee Name</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Payee ID</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Check Date</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Check $</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Check/EFT Number</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">NPI</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Patient Name</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Policy/HIC Number</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Acct Number</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">ICN</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Date of Service From</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Date of Service To</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Procedure</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Billed Amount</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Allowed Amount</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Deductible</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Coins</Data>
            </Cell>
            <Cell>
              <Data ss:Type="String">Paid Amount</Data>
            </Cell>
          </Row>
        
          <xsl:apply-templates select="FunctionGroup/Transaction"/>
        
        </Table>
        <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
          <PageSetup>
            <Header x:Margin="0.3"/>
            <Footer x:Margin="0.3"/>
            <PageMargins x:Bottom="0.75" x:Left="0.7" x:Right="0.7" x:Top="0.75"/>
          </PageSetup>
          <Unsynced/>
          <Print>
            <ValidPrinterInfo/>
            <VerticalResolution>0</VerticalResolution>
          </Print>
          <Selected/>
          <FreezePanes/>
          <FrozenNoSplit/>
          <SplitHorizontal>1</SplitHorizontal>
          <TopRowBottomPane>1</TopRowBottomPane>
          <ActivePane>2</ActivePane>
          <Panes>
            <Pane>
              <Number>3</Number>
            </Pane>
            <Pane>
              <Number>2</Number>
              <ActiveRow>0</ActiveRow>
            </Pane>
          </Panes>
          <ProtectObjects>False</ProtectObjects>
          <ProtectScenarios>False</ProtectScenarios>
        </WorksheetOptions>
      </Worksheet>
    </Workbook>
  </xsl:template>

  <xsl:template match="Transaction">
    <xsl:variable name="sequence" select="position()"/>
    <xsl:for-each select="Loop[@LoopId='2000']/Loop[@LoopId='2100']/Loop[@LoopId='2110']">
      <xsl:call-template name="Procedure">
        <xsl:with-param name="checkSequence" select="$sequence"/>
        <xsl:with-param name="row" select="position()"/>
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
    <xsl:param name="row"/>
    <xsl:param name="tran"/>
    <xsl:param name="payer"/>
    <xsl:param name="payee"/>
    <xsl:param name="header"/>
    <xsl:param name="claim"/>
    <xsl:param name="service"/>
    <Row  ss:AutoFitHeight="0">
      <xsl:if test="$row = 1">
        <xsl:attribute name="ss:StyleID">s64</xsl:attribute>
      </xsl:if>
      <xsl:if test="$row mod 2 = 0">
        <xsl:attribute name="ss:StyleID">s68</xsl:attribute>
      </xsl:if>
      <Cell>
        <Data ss:Type="String">
          <!-- Filename -->
          <xsl:value-of select="$filename"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Check Sequence -->
          <xsl:value-of select="'Check '"/>
          <xsl:value-of select="$checkSequence"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Payer Name-->
          <xsl:value-of select="$payer/N1/N102"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Payee Name-->
          <xsl:value-of select="$payee/N1/N102"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Payee ID -->
          <xsl:value-of select="$payee/N1/N104"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Check Date -->
          <xsl:call-template name="FormatDateYYYYMMDD">
            <xsl:with-param name="date" select="$tran/BPR/BPR16"/>
          </xsl:call-template>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="Number">
          <!-- Check Amount -->
          <xsl:value-of select="$tran/BPR/BPR02"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Check/EFT Number -->
          <xsl:value-of select="$tran/TRN/TRN02"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- NPI -->
          <xsl:value-of select="$claim/NM1[NM101='82' and NM108='XX']/NM109"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Patient Name -->
          <xsl:value-of select="$claim/NM1[NM101='QC']/NM103"/>
          <xsl:text>, </xsl:text>
          <xsl:value-of select="$claim/NM1[NM101='QC']/NM104"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Policy/HIC Number -->
          <xsl:value-of select="$claim/REF[REF01='1L']/REF02"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Acct Number -->
          <xsl:value-of select="$tran/BPR/BPR09"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- ICN -->
          <xsl:value-of select="$claim/CLP/CLP07"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
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
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
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
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="String">
          <!-- Procedure -->
          <xsl:value-of select="$service/SVC/SVC01/SVC0102"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="Number">
          <!-- Billed Amount -->
          <xsl:value-of select="$service/SVC/SVC02"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="Number">
          <!-- Allowed Amount -->
          <xsl:value-of select="$service/AMT[AMT01='B6']/AMT02"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="Number">
          <!-- Deductible -->
          <xsl:value-of select="$service/AMT[AMT01='KH']/AMT02"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="Number">
          <!-- Coins -->
          <xsl:value-of select="$header/TS3/TS318"/>
        </Data>
      </Cell>
      <Cell>
        <Data ss:Type="Number">
          <!-- Paid Amount -->
          <xsl:value-of select="$service/SVC/SVC03"/>
        </Data>
      </Cell>
    </Row>
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