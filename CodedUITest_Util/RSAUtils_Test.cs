﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodedUITest_Util
{
    [TestClass]
    public class RSAUtils_Test
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
            string args = @"新华社巴拿马城12月3日电 当地时间12月3日，国家主席习近平在巴拿马城同巴拿马总统巴雷拉共同参观巴拿马运河新船闸。
习近平和夫人彭丽媛抵达新船闸时，巴雷拉总统和夫人卡斯蒂略在停车处迎接。
不远处，满载集装箱的中国远洋海运集团“玫瑰轮”正在第一船闸停靠等待过闸。该轮今年10月1日从中国青岛起航，横跨太平洋，通过巴拿马运河驶经大西洋抵达美国3个港口，现正通过巴拿马运河返航。
两国元首听取了中国远洋海运集团负责人介绍“玫瑰轮”情况及中远海运集团在巴拿马运营情况。
两国元首夫妇向“玫瑰轮”船员挥手致意。船员们挥舞中巴两国国旗，向两国元首夫妇问好。
习近平同“玫瑰轮”船长通话，询问船上工作和生活条件，慰问全体船员。习近平说，很高兴在巴拿马运河同“玫瑰轮”的船长和船员们通话。希望你们善用巴拿马运河，不断优化物流运输，为促进国家航运事业和全球贸易繁荣作出更大贡献。我很关心在外远航的船员们，希望大家工作生活顺利，高高兴兴起航、平平安安回家。祝愿大家一帆风顺。
“玫瑰轮”的船长和船员们听到习近平的亲切慰问和殷切嘱托十分激动。他们感谢习近平主席对全体船员的关心和问候，表示将牢记习近平主席嘱托，以实际行动为中外经贸往来贡献力量。
随后，两国元首夫妇共同前往船闸控制塔，听取巴拿马运河管理运营情况介绍。在控制室外平台上，两国元首俯瞰新船闸和运河全景。巴雷拉向习近平详细介绍运河的历史、现状及未来发展规划。
两国元首来到控制室内，巴雷拉请习近平点击控制电脑按钮，开启船闸。随着船闸向两侧开启，“玫瑰轮”缓缓通过船闸。控制室内响起阵阵掌声。
丁薛祥、杨洁篪、王毅、何立峰等参加上述活动。
巴拿马运河是连通太平洋和大西洋的重要国际水道，1914年正式开通。1999年，巴拿马收回运营主权。2016年，运河扩建工程完工。";

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
