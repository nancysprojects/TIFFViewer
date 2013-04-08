
Partial Class ViewImg
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'impersonate for the scope of the upload process.
        Dim TheImg As System.Drawing.Image
        TheImg = New App.Files.TIF(Server.UrlDecode(Request.QueryString("FilePath"))).GetTiffImageThumb(System.Convert.ToInt16(Request.QueryString("Pg")), System.Convert.ToInt16(Request.QueryString("Height")), System.Convert.ToInt16(Request.QueryString("Width")))
        If Not TheImg Is Nothing Then
            Select Case Request.QueryString("Rotate")
                Case "90"
                    TheImg.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone)
                    Exit Select
                Case "180"
                    TheImg.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone)
                    Exit Select
                Case "270"
                    TheImg.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone)
                    Exit Select
            End Select

            Response.ContentType = "image/jpeg"
            TheImg.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            TheImg.Dispose()
        End If




    End Sub
End Class
