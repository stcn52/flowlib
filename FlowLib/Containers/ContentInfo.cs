
/*
 *
 * Copyright (C) 2007 Mattias Blomqvist, patr-blo at dsv dot su dot se
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
 *
 */

using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using FlowLib.Enums;

namespace FlowLib.Containers
{
    /// <summary>
    /// Class representing Share/Download/Upload Item.
    /// Most probably a file on your system.
    /// </summary>
    public class ContentInfo : PropertyContainer<string, string>
    {
        public const string ID = "id";
        public const string TTH = "tth";
        /// <summary>
        /// Virtual path in your/others share
        /// </summary>
        public const string VIRTUAL = "virtual";
        public const string NAME = "name";
        /// <summary>
        /// Storage path for this content
        /// </summary>
        public const string STORAGEPATH = "storage";
        public const string FILELIST = "filelist";
        public const string REQUEST = "request";   // This should only be set when we want / send content

        protected long modified = 0;
        protected long size = -1;

        protected ContentExtend extend = ContentExtend.None;
        #region For content made of files.
        /// <summary>
        /// This property is here for xml serialization only.
        /// </summary>
        [XmlIgnore()]
        public bool IsHiddenSpecified
        {
            get { return IsHidden; }
            set { }
        }
        /// <summary>
        /// This property is here for xml serialization only.
        /// </summary>
        [XmlIgnore()]
        public bool IsSystemSpecified
        {
            get { return IsSystem; }
            set { }
        }
        /// <summary>
        /// This property is here for xml serialization only.
        /// </summary>
        [XmlIgnore()]
        public bool IdSpecified
        {
            get { return false; }
            set { }
        }
        /// <summary>
        /// Indicates if this content is a filelist
        /// </summary>
        [XmlIgnore()]
        public bool IsFilelist
        {
            get { return ContainsKey(FILELIST); }
        }

        /// <summary>
        /// Indicates that file that this info was created from
        /// had the System.IO.FileAttributes.Hidden attribute set.
        /// </summary>
        public bool IsHidden
        {
            get { return ((extend | ContentExtend.Hidden) == extend); }
            set {
                if (((extend | ContentExtend.Hidden) == extend) != value)
                    extend = ~ContentExtend.Hidden;
            }
        }
        /// <summary>
        /// Indicates that file that this info was created from
        /// had the System.IO.FileAttributes.System attribute set.
        /// </summary>
        public bool IsSystem
        {
            get { return ((extend | ContentExtend.System) == extend); }
            set {
                if (((extend | ContentExtend.System) == extend) != value)
                    extend = ~ContentExtend.System;
            }
        }
        #endregion
        /// <summary>
        /// Indicates that the id of this contentinfo is some sort of a hash.
        /// It could be a tiger tree hash but it doesnt have to be.
        /// </summary>
        [XmlIgnore()]
        public bool IsHashed
        {
            get
            { return ContainsKey(TTH); }
        }
        /// <summary>
        /// Indicates that the id of this contentinfo is a TTH (Tiger Tree Hash)
        /// </summary>
        [XmlIgnore()]
        public bool IsTth
        {
            get { return ContainsKey(TTH); }
        }

        //public string SystemPath
        //{
        //    get
        //    {
        //        return Get(STORAGEPATH);
        //    }
        //    set
        //    {
        //        Set(STORAGEPATH, value);
        //    }
        //}
        /// <summary>
        /// DateTime tick on when file was last changed
        /// </summary>
        public long LastModified
        {
            get { return modified; }
            set { modified = value; }
        }
        /// <summary>
        /// Key name of this contentinfo
        /// </summary>
        public string Id
        {
            get
            {
                if (ContainsKey(ID))
                {
                    // We want to override id order.
                    return ID;
                }
                if (ContainsKey(TTH))
                    return TTH;
                else if (ContainsKey(VIRTUAL))
                    return VIRTUAL;
                else
                    return NAME;
            }
        }
        /// <summary>
        /// File size of target
        /// </summary>
        public long Size
        {
            get { return size; }
            set { size = value; }
        }

        /// <summary>
        /// Dummy Constructor for Xml Serlization
        /// </summary>
        public ContentInfo()
        {

        }

        /// <summary>
        /// Creating ContentInfo with key and its value
        /// </summary>
        /// <param name="key">Key to add value for</param>
        /// <param name="value">value that should be added</param>
        public ContentInfo(string key, string value)
        {
            Add(key, value);
        }
    }
}