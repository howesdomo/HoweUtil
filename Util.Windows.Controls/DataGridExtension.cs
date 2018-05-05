using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Controls
{
    public static class DataGridExtension
    {
        /// <summary>
        /// 区域粘贴
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datagrid"></param>
        public static void AreaPaste<T>(this System.Windows.Controls.DataGrid datagrid)
        {
            if (datagrid == null)
            {
                throw new Exception("DataGrid为空");
            }

            if (datagrid.SelectedCells == null || datagrid.SelectedCells.Count <= 0)
            {
                throw new Exception("未选中任何单元格。");

            }

            if (datagrid.CurrentCell == null)
            {
                throw new Exception("未选中任何单元格。");
            }

            int datagridSelectedCellsCount = datagrid.SelectedCells.Count;

            List<string> clipBoardContentList = new List<string>();

            #region 粘贴区域

            string pasteText = System.Windows.Clipboard.GetText();

            if (string.IsNullOrEmpty(pasteText) == true)
            {
                throw new Exception("剪贴板无信息。");
            }

            if (pasteText.Contains('\r') == true || pasteText.Contains('\n') == true)
            {
                List<string> splited = pasteText.Split('\r', '\n')
                .Select(i => i.Trim())
                .Where(i => string.IsNullOrEmpty(i) == false)
                .ToList();
                ;

                clipBoardContentList.Clear();
                clipBoardContentList.AddRange(splited);
            }
            else
            {
                clipBoardContentList.Add(pasteText);
            }

            #endregion 粘贴区域

            if (datagridSelectedCellsCount != clipBoardContentList.Count)
            {
                var errorMsg = string.Format("无法粘贴。选中{0}格, 粘贴{1}格。", datagridSelectedCellsCount, clipBoardContentList.Count);
                throw new Exception(errorMsg);
            }

            // Cell ==> Prop
            System.Windows.Controls.DataGridColumn column = datagrid.CurrentCell.Column;

            // column.Binding.Path.Path // 待解决 无法获取Binding 属性, 暂时先用默认的 SortMemberPath 进行处理
            string matchField = column.SortMemberPath;

            for (int index = 0; index < clipBoardContentList.Count; index++)
            {

                DataGridCellInfo cellInfo = datagrid.SelectedCells[index];
                T model = (T)cellInfo.Item;

                Type type = model.GetType();
                foreach (System.Reflection.PropertyInfo propInfo in type.GetProperties())
                {
                    if (propInfo.Name == matchField)
                    {
                        string argExMessage = string.Empty;
                        int convertToXIndex = -1;

                        try
                        {
                            propInfo.SetValue
                            (
                                obj: model,
                                value: clipBoardContentList[index],
                                index: null
                            );
                            continue; // --> foreach (System.Reflection.PropertyInfo propInfo in type.GetProperties())
                        }
                        catch (System.ArgumentException argEx)
                        {
                            // 报错信息 "类型“System.String”的对象无法转换为类型“System.Decimal”。"
                            // 报错信息 "类型“System.String”的对象无法转换为类型“System.Int32”。" 等等
                            convertToXIndex = argEx.Message.LastIndexOf("System.");

                            if (convertToXIndex < 0)
                            {
                                // 不是以上无法转换的报错，抛出错误
                                throw argEx;
                            }
                            else
                            {
                                argExMessage = argEx.Message;
                            }
                        }

                        #region 慢慢完善各种类型

                        if (string.IsNullOrEmpty(argExMessage) == false)
                        {
                            string convertToX = string.Empty;
                            try
                            {


                                convertToX = argExMessage.Substring(convertToXIndex);
                                convertToX = convertToX
                                    .Replace(',', ' ')
                                    .Replace('"', ' ')
                                    .Replace('”', ' ')
                                    .Replace('。', ' ')
                                    ;

                                convertToX = convertToX.Trim();
                                switch (convertToX.ToUpper())
                                {
                                    case "SYSTEM.INT32":
                                        {
                                            int valueAfterConvert = Convert.ToInt32(clipBoardContentList[index]);
                                            propInfo.SetValue(model, valueAfterConvert, null);
                                            break;
                                        }

                                    case "SYSTEM.DECIMAL":
                                        {
                                            decimal valueAfterConvert = Convert.ToDecimal(clipBoardContentList[index]);
                                            propInfo.SetValue(model, valueAfterConvert, null);
                                        }
                                        break;

                                    default:
                                        throw new Exception(argExMessage);
                                }

                                continue; // --> foreach (System.Reflection.PropertyInfo propInfo in type.GetProperties())

                            }
                            catch (Exception toAddInfoEx)
                            {
                                throw new Exception
                                (
                                    string.Format("{0}\r\n剪贴板第 {1} 格\r\n输入的字符串：{2}\r\n转换的格式：{3}",
                                        toAddInfoEx.Message,
                                        (index + 1),
                                        clipBoardContentList[index],
                                        convertToX)
                                );
                            }
                        }

                        #endregion 慢慢完善各种类型
                    }
                }
            }
        }

        // DataGrid添加菜单后, 调用区域粘贴
        //private void HowToUseAreaPaste()
        //{
        //ContextMenu ctMenu = new ContextMenu();

        //List<MenuItem> l = new List<MenuItem>();
        //MenuItem pasteMenuItem = new MenuItem();
        //pasteMenuItem.Header = "粘贴";
        //pasteMenuItem.Click += (o, e) =>
        //{
        //    try
        //    {
        //        sgv.DataGrid.AreaPaste<Model.DeliveryOrderModel>();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //};
        //l.Add(pasteMenuItem);

        //ctMenu.ItemsSource = l;
        //this.sgv.DataGrid.ContextMenu = ctMenu;
        //}
    }
}
