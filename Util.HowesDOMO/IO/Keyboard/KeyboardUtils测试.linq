<Query Kind="Program">
  <Reference>D:\SC_Github\HoweUtil\Util.HowesDOMO\bin\Debug\Newtonsoft.Json.dll</Reference>
  <Reference>D:\SC_Github\HoweUtil\Util.HowesDOMO\bin\Debug\Util.HowesDOMO.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
</Query>

void Main()
{	
	// 测试模拟键盘按钮();
	// 收到串口网口信息输出到光标();
	// 截取屏幕截图();	
}

void 测试模拟键盘按钮()
{
	Thread.Sleep(3000);

	bool isTest = true;

	#region A-Z

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.Q);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.W);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.E);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.R);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.T);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.Y);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.U);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.I);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.O);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.P);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.A);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.S);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.F);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.G);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.H);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.J);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.K);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.L);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.Z);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.X);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.C);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.V);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.B);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.N);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.M);
	}

	#endregion

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_UP);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_DOWN);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_LEFT);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_RIGHT);
	}

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D0);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D1);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D2);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D3);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D4);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D5);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D6);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D7);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D8);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.D9);
	}

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMLOCK);

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD0);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD1);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD2);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD3);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD4);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD5);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD6);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD7);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD8);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NUMPAD9);

		// 小键盘的加减乘除
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_ADD);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_SUBTRACT);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_MULTIPLY);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_DIVIDE);

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_RETURN); // 小键盘 与 退格键后面的是用同一个

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_DECIMAL); // 小键盘 小数点		
	}

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F1);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F2);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F3);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F4);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F5);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F6);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F7);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F8);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F9);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F10);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F11);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_F12);
	}

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_ESCAPE);
      Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_3);  // ~
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_TAB);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_CAPITAL);

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_SHIFT);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_CONTROL);

		// Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_LWIN); // 不测试win键

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_MENU); // ALT
																			 // Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_LMENU); // ALT
																			 // Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_RMENU); // ALT

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_SPACE);

		// Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_RWIN); // 不测试win键	
		// Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_APPS); // 弹出右键菜单按钮 // 不测试菜单 
	}

	if (isTest)
	{
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_SNAPSHOT); // PrtScr
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_SCROLL);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_PAUSE);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_INSERT);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_HOME);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_PRIOR); // Page Up
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_DELETE);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_END);
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_NEXT); // Page Down
	}

	if (isTest)
	{
		// OEM 集合

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_MINUS); // 字母上方的 -
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_PLUS);  // 字母上方的 +
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_BACK);

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_4); // [{ 左中括号 左花括号
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_6); // ]} 右中括号 右花括号

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_1); // 分号 冒号 ;:
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_7); // 单双引号 '"
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_5); // 反斜杠 竖线

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_COMMA); // 逗号
		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_PERIOD); // 句号		

		Util.IO.KeyboardUtils.ExecuteForTest(Util.IO.KeyboardUtils.VK_OEM_2);  // / ?
	}


}

void 收到串口网口信息输出到光标()
{
	string fakeReceiveContent = "123中国人\r\n456普通话";
	System.Windows.Forms.Clipboard.SetText(fakeReceiveContent);
	Util.IO.KeyboardUtils.ShortcutKey_CtrlV();
}

void 截取屏幕截图()
{
	Util.IO.KeyboardUtils.PrtScr();
	Thread.Sleep(1000);
	var image = System.Windows.Forms.Clipboard.GetImage();
	if (image != null)
	{
		var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"Screenshot_{DateTime.Now.ToString("yyyy_mm_dd_HH-mm-ss")}.png");
		image.Save(path, System.Drawing.Imaging.ImageFormat.Png);
	}
}