// FFXIVAPP.Plugin.Parse ~ DTPSWidgetViewModel.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FFXIVAPP.Plugin.Parse.Models;

namespace FFXIVAPP.Plugin.Parse.Windows
{
    internal sealed class DTPSWidgetViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static Lazy<DTPSWidgetViewModel> _instance = new Lazy<DTPSWidgetViewModel>(() => new DTPSWidgetViewModel());
        private ParseEntity _parseEntity;

        public static DTPSWidgetViewModel Instance
        {
            get { return _instance.Value; }
        }

        public ParseEntity ParseEntity
        {
            get { return _parseEntity ?? (_parseEntity = new ParseEntity()); }
            set
            {
                _parseEntity = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Declarations

        #endregion

        #region Loading Functions

        #endregion

        #region Utility Functions

        #endregion

        #region Command Bindings

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
