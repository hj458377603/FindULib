using FindULib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace FindULib.ViewModels
{
    public class BookListViewModel : BaseViewModel
    {
        #region 字段

        #endregion

        #region 属性

        #endregion

        #region Command

        public ICommand SearchBookListCommand
        {
            get
            {
                return new BaseCommand(SearchBookList);
            }
        }
        public ICommand GetBookInfoCommand
        {
            get
            {
                return new BaseCommand(GetBookInfo);
            }
        }

        #endregion

        #region 方法
        private void SearchBookList()
        {

        }

        private void GetBookInfo()
        {

        }

        #endregion

    }
}
