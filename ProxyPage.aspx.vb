'***************************************************
Imports System.IO
Imports ConverterUtility.FileConverter
Imports System.Drawing


Partial Class ProxyPage
    Inherits System.Web.UI.Page




#Region "image version4.0  purpose: to display multi-page tiff images in browser"
    'Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
    Dim DefaultThumbHeight As Int16 = 100
    Dim DefaultThumbWidth As Int16 = 100
    Dim DefaultBigHeight As Int16 = 1000
    Dim DefaultBigWidth As Int16 = 1000

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Panel1 As New System.Web.UI.WebControls.Panel

        Me.Controls.Add(Panel1)

        'Add and configure the thumbnails

        FilePath = Server.UrlDecode(UNR.UtilClasses.DecryptString(Request.QueryString("uncpath"), True))

        Dim StartPg As Integer
        Dim EndPg As Integer
        Dim BigImgPg As Integer
        Dim PagerSize = 20

        '//Determine Start/End Pages 
        StartPg = 1
        'Determine Start/End Page
        BigImgPg = StartPg
        EndPg = StartPg + (PagerSize - 1)


        If EndPg > TotalTIFPgs Then
            EndPg = TotalTIFPgs
        End If

        'Add/ configure the thumbnails
        While StartPg <= EndPg
            Dim lbl = New Label()
            If StartPg Mod 1 = 0 And StartPg <> 0 Then
                lbl.text = "&nbsp<br />"
            Else
                lbl.text = "&nbsp"
            End If
            Dim thumbimage As New System.Web.UI.WebControls.Image()
            thumbimage.BorderStyle = BorderStyle.Solid
            thumbimage.BorderWidth = Unit.Parse("1")
            thumbimage.Attributes.Add("onClick", "ChangePg(" + StartPg.ToString() + ");")
            thumbimage.Attributes.Add("onmouseover", "this.style.cursor = 'hand';")
            thumbimage.ImageUrl = "ViewImg.aspx?FilePAth=" & Server.UrlEncode(FilePath) & "&Pg=" & StartPg.ToString() & "&Height=" & DefaultThumbHeight.ToString() & "&Width=" & DefaultBigWidth
            _plcImgsThumbs.Controls.Add(thumbimage)
            _plcImgsThumbs.Controls.Add(lbl)
            StartPg += 1
        End While

        'Bind big image 
        Dim BigImg As New System.Web.UI.WebControls.Image()
        BigImg.BorderStyle = BorderStyle.Solid
        BigImg.BorderWidth = Unit.Parse("1")
        BigImg.ID = "_imgBig"
        BigImg.ImageUrl = "ViewImg.aspx?View=1&FilePath=" + FilePath + "&Pg=" + BigImgPg.ToString() + "&Height=" + DefaultBigHeight.ToString() + "&Width=" + DefaultBigWidth.ToString()
        _plcBigImg.Controls.Add(BigImg)


    End Sub

    Public Property TotalTIFPgs() As Int16
        Get
            If ViewState("TotalTIFPgs") = Nothing Then
                Dim TheFile As New App.Files.TIF(FilePath)
                ViewState("TotalTIFPgs") = TheFile.PageCount
                TheFile.Dispose()
            End If
            Return System.Convert.ToInt16(ViewState("TotalTIFPgs"))
        End Get
        Set(ByVal value As Int16)
            ViewState("TotalTIFPgs") = value
        End Set
    End Property
    'Get file path
    Public Property FilePath() As String
        Get
            If ViewState("FilePath") = Nothing Then
                Return ""
            End If
            Return ViewState("FilePath").ToString
        End Get
        Set(ByVal value As String)
            ViewState("FilePath") = value
        End Set
    End Property
#End Region



End Class
