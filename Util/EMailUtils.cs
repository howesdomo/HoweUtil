using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class EMailUtils
    {
        public void SendEMailAsync
        (
            Action<Task> handler,
            List<string> receiverList,
            string subject,
            string content,
            List<string> attachmentPathList = null,
            SmtpClient smtp = null,
            MailPriority mailPriority = MailPriority.Normal
        )
        {
            Task task = new Task(() => SendEMail(receiverList, subject, content, attachmentPathList, smtp, mailPriority));
            task.ContinueWith(handler);
            task.Start();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="receiverList">收件人 + 抄送人地址集合</param>
        /// <param name="subject">主题</param>
        /// <param name="content">内容</param>
        /// <param name="attachmentPathList">附件文件地址集合</param>
        /// <param name="smtp">自定义配置</param>
        /// <param name="mailPriority">设置邮件优先级</param>
        public void SendEMail
        (
            List<string> receiverList,
            string subject,
            string content,
            List<string> attachmentPathList = null,
            SmtpClient smtp = null,
            MailPriority mailPriority = MailPriority.Normal
        )
        {
            if (receiverList == null || receiverList.Count == 0)
            {
                throw new Exception("至少需要一位收件人");
            }

            #region SmtpClient

            if (smtp == null)
            {
                smtp = new System.Net.Mail.SmtpClient();

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; //将smtp的出站方式设为 Network
                smtp.EnableSsl = false;//smtp服务器是否启用SSL加密
                smtp.Host = "smtp.qiye.163.com"; //指定 smtp 服务器地址
                smtp.Port = 25;             //指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去

                //如果你的SMTP服务器不需要身份认证，则使用下面的方式，不过，目前基本没有不需要认证的了
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential("howe@enpot.com.cn", "13016061994Xg");
            }

            #endregion

            MailMessage mailMessage = new MailMessage();

            #region MailMessage

            mailMessage.Priority = mailPriority;
            Encoding emailEncoding = Encoding.GetEncoding("gb2312");
            mailMessage.From = new MailAddress("howe@enpot.com.cn", "howe@enpot.com.cn", emailEncoding);

            for (int i = 0; i < receiverList.Count; i++)
            {
                mailMessage.To.Add(new MailAddress(receiverList[i], receiverList[i], emailEncoding));

                //if (i == 0) // 收件人
                //{
                //    mailModel.To.Add(new MailAddress(to_emailAddress[i], to_emailAddress[i], emailEncoding));
                //}
                //else // 抄送者
                //{
                //    mailModel.To.Add(new MailAddress(to_emailAddress[i], to_emailAddress[i], emailEncoding));
                //}
            }

            mailMessage.Subject = subject;
            mailMessage.SubjectEncoding = emailEncoding;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = emailEncoding;
            mailMessage.Body = content;

            if (attachmentPathList != null && attachmentPathList.Count > 0)
            {
                attachmentPathList.ForEach(i => mailMessage.Attachments.Add(new Attachment(i)));
            }

            #endregion

            smtp.Send(mailMessage);
        }
    }

    public class EMailConfig
    {
        SmtpClient SmtpClient { get; set; }
    }
}
