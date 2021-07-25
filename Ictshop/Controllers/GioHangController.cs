﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ictshop.Models;
using Ictshop.PaymentMoMo;
using Newtonsoft.Json.Linq;
using PayPal.Api;

namespace Ictshop.Controllers
{

    public class GioHangController : Controller
    {
        Qlbanhang db = new Qlbanhang();
        /**
         * Hello Tao am Ho Minh 
         * Chức năng giỏ hàng**/
        #region Giỏ Hàng
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Thêm giỏ hàng
        public ActionResult ThemGioHang(int iMasp, string strURL)
        {
            Sanpham sp = db.Sanphams.SingleOrDefault(n => n.Masp == iMasp);
            if ( sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp này đã tồn tại trong session[giohang] chưa
            GioHang sanpham = lstGioHang.Find(n => n.iMasp == iMasp);
            if (sanpham == null)
            {
                sanpham = new GioHang(iMasp);
                //Add sản phẩm mới thêm vào list
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strURL);
            }
        }
        //Cập nhật giỏ hàng 
        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            //Kiểm tra masp
            Sanpham sp = db.Sanphams.SingleOrDefault(n => n.Masp== iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<GioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMasp == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int iMaSP)
        {
            //Kiểm tra masp
            Sanpham sp = db.Sanphams.SingleOrDefault(n => n.Masp== iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMasp == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMasp == iMaSP);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("SuaGioHang");
        }
        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            List<GioHang> listGioHang = Session["GioHang"] as List<GioHang>;

            if (listGioHang == null || listGioHang.Count == 0 )
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        //Tính tổng số lượng và tổng tiền
        //Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
        //tạo partial giỏ hàng
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xây dựng 1 view cho người dùng chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);

        }
        #endregion

        /**
         * Hello Tao am Ho Minh 
         * Chức năng đặt hàng**/
        #region Đặt hàng
        //Xây dựng chức năng đặt hàng
        [HttpPost]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng đăng nhập
            if (Session["use"] == null || Session["use"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "User");
            }
            //Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            //Thêm đơn hàng
            Nguoidung kh = (Nguoidung)Session["use"];

            var ddh = new Donhang()
            {
                MaNguoidung = kh.MaNguoiDung,
                Ngaydat = DateTime.Now,
                Tinhtrang = 0,
                MaDonMoMo = "DONTRUCTIEP"
            };

            List<GioHang> gh = LayGioHang();
            db.Donhangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                Chitietdonhang ctDH = new Chitietdonhang();
                ctDH.Madon = ddh.Madon;
                ctDH.Masp = item.iMasp;
                ctDH.Soluong = item.iSoLuong;
                ctDH.Dongia = (decimal)item.dDonGia;
                db.Chitietdonhangs.Add(ctDH);
            }
            db.SaveChanges();
            gh.Clear();
            return RedirectToAction("DatHangThanhCong");
        }

        public ActionResult DatHangThanhCong()
        {
            return View();
        }
        #endregion

        /**
         * Hello Tao am Ho Minh 
         * Chức năng thanh toán ví MoMo**/
        #region Thanh Toán MoMo
        public ActionResult ThanhToanMomo()
        {
            /**
             * Mày thanh toán trên 50tr thì méo cho nhóe
             * Khách hàng vip tao mới cho con trai của tao ạ :3
             * Khi nào t hứng thì t tăng hạn mức gia dịch nhóe
             * **/

            if(TongTien() <= 50000000)
            {
                /**
                 * Tao kiểm tra đơn hàng
                 * và tao kiểm tra giỏ hàng ok :))
                 * **/
                if (Session["use"] == null || Session["use"].ToString() == "")
                {
                    return RedirectToAction("Dangnhap", "User");
                }
                if (Session["GioHang"] == null)
                {
                    RedirectToAction("Index", "Home");
                }

                /**
                 * Trường hợp thêm sản phẩm trước khi người dùng quét mã.
                 * User ơi mày sẽ không thoát được đâu con trai :)) mày đừng có mà lươn :#
                 * Cái này tao gặp nhiều rồi :))
                 * **/

                string endpoint = ConfigurationManager.AppSettings["endpoint"].ToString();
                string accessKey = ConfigurationManager.AppSettings["accessKey"].ToString();
                string serectKey = ConfigurationManager.AppSettings["serectKey"].ToString();
                string orderInfo = "DH" + DateTime.Now.ToString("yyyyMMHHmmss");                
                string returnUrl = ConfigurationManager.AppSettings["returnUrl"].ToString();
                string notifyurl = ConfigurationManager.AppSettings["notifyUrl"].ToString();
                string partnerCode = ConfigurationManager.AppSettings["partnerCode"].ToString();

                /**
                 * Lấy giò hàng
                 * Thêm đơn hàng vào CSDL
                 * **/

                List<GioHang> gh = LayGioHang();
                Nguoidung kh = (Nguoidung)Session["use"];
                var ddh = new Donhang()
                {
                    MaNguoidung = kh.MaNguoiDung,
                    Ngaydat = DateTime.Now,
                    Tinhtrang = 3,
                    MaDonMoMo = orderInfo
                };
                db.Donhangs.Add(ddh);
                db.SaveChanges();

                /**
                 * Dòng này tao thêm sản phẩm vào bảng 
                 * Chi tiết đơn hàng ok :))
                 * **/

                foreach (var item in gh)
                {
                    Chitietdonhang ctDH = new Chitietdonhang();
                    ctDH.Madon = ddh.Madon;
                    ctDH.Masp = item.iMasp;
                    ctDH.Soluong = item.iSoLuong;
                    ctDH.Dongia = (decimal)item.dDonGia;
                    db.Chitietdonhangs.Add(ctDH);
                }
                db.SaveChanges();

                /**
                 * Bắt đầu xác thực Momo
                 * Thoát làm sao được con trai của ta :))
                 * **/

                string amount = TongTien().ToString();
                string orderid = ddh.Madon.ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                MoMoSecurity Cryto = new MoMoSecurity();
                string signature = Cryto.signSHA256(rawHash, serectKey);

                JObject message = new JObject
                {
                    { "partnerCode", partnerCode },
                    { "accessKey", accessKey },
                    { "requestId", requestId },
                    { "amount", amount},
                    { "orderId", orderid },
                    { "orderInfo", orderInfo },
                    { "returnUrl", returnUrl },
                    { "notifyUrl", notifyurl },
                    { "extraData", extraData },
                    { "requestType", "captureMoMoWallet" },
                    { "signature", signature }
                };

                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);
                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
            return RedirectToAction("GioHang");
        }

        /**
        * Tao viết hàm này để khi redirect về trang này
        * thì nó sẽ làm gì còn lâu tao mới nói
        * thằng nào đọc code này của tao thì tự lên doc MoMo mà đọc nhóe :3 
        * **/

        public ActionResult ReturnUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);

            /**
             * Tao kiểm tra chữ kí, tao kí ... fan 2k3 :3
             * **/

            if (signature != Request["signature".ToString()])
            {
                ViewBag.message = "THÔNG TIN REQUEST KHÔNG HỢP LỆ ";
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "THANH TOÁN THẤT BẠI";
            }
            else
            {
                ViewBag.message = "THANH TOÁN THÀNH CÔNG";
                string orderId = Request.QueryString["orderId"].ToString();
                string orderInfo = Request.QueryString["orderInfo"].ToString();

                List<GioHang> gh = LayGioHang();

                /**
                 * Dòng này t viết ra để update đơn hàng
                 * Nhìn vào k biết thì nghỉ mọe đi :)
                 * **/

                if (gh.Count > 0)
                {
                    var suaDonHang = db.Donhangs.Find(int.Parse(orderId));
                    suaDonHang.MaDonMoMo = orderInfo;
                    suaDonHang.Tinhtrang = 1;
                    db.SaveChanges();
                    gh.Clear();
                }
                gh.Clear();
            }
            return View();
        }

        public ActionResult NotifyUrl()
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectKey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectKey);

            if (signature != Request["signature".ToString()])
            {
                /**
                 * Hàm cha của mày k gọi nên t méo thích viết nữa 
                 * tao mệt rồi cảm ơn :)
                 * **/
            }
            string status_code = Request["status_code"].ToString();


            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DatHangMoMoThanhCong()
        {
            return View();
        }

        public ActionResult DatHangMoMoThatBai()
        {
            return View();
        }
        #endregion

        #region Thanh Toán Paypal
        public Payment payment;

        public Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<Item>() };

            List<GioHang> gh = LayGioHang();
            foreach(var cart in gh)
            {
                listItems.items.Add(new Item()
                {
                    name = cart.sTensp,
                    currency = "USD",
                    price = cart.dDonGia.ToString(),
                    quantity = cart.iSoLuong.ToString(),
                    sku = "sku",
                }); 
            }

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrl = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = TongTien().ToString()
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Ho Minh test transaction",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems,
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrl
            };
            return payment.Create(apiContext);

        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExcution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment()
            {
                id = paymentId
            };
            return payment.Execute(apiContext, paymentExcution);
        }

        public ActionResult ThanhToanPaypal()
        {
            APIContext apicontext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if(string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/ThanhToanPaypal";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apicontext, baseURI + "guid=" + guid);

                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = link.href;
                        }
                        Session.Add(guid, createdPayment.id);
                        return Redirect(paypalRedirectUrl);
                    }
                } else
                {
                    var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apicontext, payerId, Session[guid] as string);
                    if(executePayment.state.ToLower() != "approved")
                    {
                        return View("ThanhToanPaypalFail");
                    };
                }
            } catch(Exception ex)
            {
                throw;
            }
            return View("ThanhToanPaypalSuccess");
        }
        #endregion

    }
}