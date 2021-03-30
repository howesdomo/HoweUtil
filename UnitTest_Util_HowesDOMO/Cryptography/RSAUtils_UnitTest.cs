using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_Util_HowesDOMO.Cryptography
{
    [TestClass]
    public class RSAUtils_UnitTest
    {
        [TestMethod]
        public void Test_RSA()
        {
            string args = "abc一二三123";

            string miwen = Util.Cryptography.RSAUtils.Encrypt2Base64Str(args);
            string r = Util.Cryptography.RSAUtils.Decrypt2String(miwen);

            Assert.AreEqual(args, r);
        }

        [TestMethod]
        public void Test_RSA_MoreTextLen()
        {
            string args = @"NBA常规赛继续进行，布鲁克林篮网队（32胜15负）延续胜势。哈登得到38分、11个篮板和13次助攻，欧文得到27分和7个篮板，他们在第四节关键时刻四罚全中为球队锁定胜局，篮网队在主场以112-107险胜明尼苏达森林狼队（11胜36负）。篮网队拿到2连胜，他们结束了对阵森林狼队的5连败，森林狼队遭遇2连败。
篮网队的哈登得到38分、11个篮板和13次助攻，欧文得到27分和7个篮板，乔丹得到10分，布朗得到10分、5个篮板和5次助攻，格林得到10分和4个篮板。森林狼队的唐斯得到31分、12个篮板、5次助攻和3次盖帽，爱德华兹得到23分和10个篮板，麦克丹尼尔斯得到13分、7个篮板和4次助攻，比斯利得到9分和4次助攻。
篮网队的欧文复出回到首发阵容。开赛后两队交替领跑，哈里斯三分中的率队打出7-0的小高潮，首节进行了4分30秒时篮网队以15-10领先。比斯利还击三分，欧文切入进攻模式，连进3个三分球，他连得13分帮助球队扩大优势。卢比奥两罚还击，格里芬和约翰逊联手5分，哈登也突破得手，他们带领球队以7-3的小高潮结束首节，篮网队以37-25领先12分。
格林在第二节开始后独取5分，篮网队继续扩大优势。麦克丹尼尔斯还击5分，他率队连追7分，森林狼队以34-44落后。欧文连进两球稳住局势，麦克丹尼尔斯和比斯利分别投中三分，他们引领球队又追8分，半场前4分35秒时森林狼队以42-48落后。布朗连造犯规四罚全中，哈登也突破得分，这为篮网队稳住局势。之后两队各打成几次进攻，爱德华兹空接上篮结束第二节，森林狼队以52-61落后9分。
篮网队的欧文上半场得到19分，哈登得到12分和8次助攻；森林狼队的唐斯得到12分、5个篮板和4次助攻。
哈登在第三节开始后不久连拿5分，这帮助篮网队把优势拉开到13分。爱德华兹连投带罚还击6分，诺维尔也命中三分，他们率队打出9-0的反击波，森林狼队追至66-70。哈登再次开火又拿6分稳住局势，森林狼队非常顽强又追5分，欧文跳投命中，格里芬打3分成功，第三节还有1分30秒时篮网队以85-77领先。诺维尔突破被封盖，哈登突破得手，三节结束时篮网队以87-77领先10分。
哈登用三分球揭开第四节大幕，森林狼队外线毫不逊色，比斯利、麦克劳林和唐斯各中三分，第四节进行了5分钟时他们追至90-96。哈登保持攻击性还击5分，欧文也跑投命中，篮网队把优势重新拉开到两位数。唐斯和爱德华兹联手4分，欧文跳投命中，唐斯再中三分，爱德华兹也两次单打得手，他们率队打出9-0的反击波，第四节还有58.6秒时森林狼队追至107-108。
欧文跳投不中，森林狼队请求暂停，爱德华兹突破抛投不中，布朗拿到篮板，森林狼队采取犯规战术。哈登两罚全中，第四节还有14秒时篮网队以110-107领先。森林狼队暂停后比斯利右翼三分不中，欧文拿到篮板被犯规，两罚全中，篮网队以112-107领先。唐斯右翼三分不中，哈里斯拿到篮板，篮网队主场获胜。";

            string miwen = Util.Cryptography.RSAUtils.Encrypt2Base64Str(args);
            string r = Util.Cryptography.RSAUtils.Decrypt2String(miwen);

            Assert.AreEqual(args, r);
        }

        [TestMethod]
        public void Test_RSA_StringEmpty()
        {
            string args = string.Empty;

            string miwen = Util.Cryptography.RSAUtils.Encrypt2Base64Str(args);
            string r = Util.Cryptography.RSAUtils.Decrypt2String(miwen);

            Assert.AreEqual(args, r);
        }

    }
}
