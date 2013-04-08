<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProxyPage.aspx.vb" Inherits="ProxyPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Documents</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Table runat="server" ID="_tblImgs" Height="100%" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="15%" BorderColor="Black" BorderStyle="Solid" BorderWidth="2">
                        <asp:Label runat="server" ID="_lblClickNote" Text="Click to view Bigger" Font-Size="Small" /><br />
                        <asp:PlaceHolder runat="server" ID="_plcImgsThumbs"  />
                        <br />
                        <asp:PlaceHolder runat="server" ID="_plcImgsThumbsPager" />
                    </asp:TableCell>
                    <asp:TableCell Width="85%">
                        <asp:PlaceHolder runat="server" ID="_plcBigImg" />
                        <br />
                      <asp:Label runat="server" ID="Label1" Text="actions based off original page" Font-Size="Small" /><br />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
   <script language="javascript" type="text/javascript">
       function ChangePg(Pg) {
           Src = 'ViewImg.aspx?View=1&FilePath=' + GetBigSrc("FilePath") + "&Pg=" + Pg + "&Height=" + GetBigSrc("Height") + "&Width=" + GetBigSrc("Width");
           SrcBig = 'ViewImg.aspx?View=1&FilePath=' + GetBigSrc("FilePath") + "&Pg=" + Pg + "Height=" + 1500 + "&Width=" + 100;
           SrcRevert = 'ViewImg.aspx?View=1&FilePath=' + +GetBigSrc("FilePath") + "&Pg=" + Pg + "&Height=" + 600 + "&Width=" + 600;
           document.getElementById('_imgBig').src = Src;
           document.getElementById('_hlRot270').onclick = function () { ChangePg(Pg); document.getElementById('_imgBig').src = Src + "&Rotate=270"; };
           document.getElementById('_hlRot180').onclick = function () { ChangePg(Pg); document.getElementById('_imgBig').src = Src + "&Rotate=180"; };
           document.getElementById('_hlRot90').onclick = function () { ChangePg(Pg); document.getElementById('_imgBig').src = Src + "&Rotate=90"; };
           document.getElementById('_hlBig').onclick = function () { ChangePg(Pg); document.getElementById('_imgBig').src = SrcBig + "&Rotate" + GetBigSrc("Rotate"); };
           document.getElementById('_hlSmall').onclick = function () { ChangePg(Pg); document.getElementById('_imgBig').src = SrcRevert; };
       }

       function GetBigSrc(Qrystr) {
           var Qry = document.getElementById('_imgBig').src;
           //alert(Qry);
           gy = Qry.split("&");
           for (i = 0; i < gy.length; i++) {
               ft = gy[i].split("=");
               if (ft[0] == Qrystr)
                   return ft[1];
           }
       }
    </script>
</body>
</html>
