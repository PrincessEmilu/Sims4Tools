﻿/***************************************************************************
 *  Copyright (C) 2014 by Inge Jones                                       *
 *                                                                         *
 *                                                                         *
 *  This file is part of the Sims 4 Package Interface (s4pi)               *
 *                                                                         *
 *  s4pi is free software: you can redistribute it and/or modify           *
 *  it under the terms of the GNU General Public License as published by   *
 *  the Free Software Foundation, either version 3 of the License, or      *
 *  (at your option) any later version.                                    *
 *                                                                         *
 *  s4pi is distributed in the hope that it will be useful,                *
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of         *
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the          *
 *  GNU General Public License for more details.                           *
 *                                                                         *
 *  You should have received a copy of the GNU General Public License      *
 *  along with s4pi.  If not, see <http://www.gnu.org/licenses/>.          *
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using s4pi.Interfaces;
 
namespace CatalogResource
{
    public class CFTRResource : AResource
    {
        #region Attributes

        const int kRecommendedApiVersion = 1;
        public override int RecommendedApiVersion { get { return kRecommendedApiVersion; } }
        private uint version = 0x7;
        private CatalogCommon commonA;
        private TGIBlock trimRef;
        private uint materialVariant;
        private TGIBlock modlRef;
        private ulong swatchGrouping;
        private ColorList colors;
        private uint unk01;

        #endregion Attributes ================================================================

        #region Content Fields

        [ElementPriority(0)]
        public string TypeOfResource
        {
            get { return "CTProductFountainTrim"; }
        }
        [ElementPriority(1)]
        public uint Version
        {
            get { return version; }
            set { if (version != value) { version = value; this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(2)]
        public CatalogCommon CommonBlock
        {
            get { return commonA; }
            set { if (!commonA.Equals(value)) { commonA = new CatalogCommon(kRecommendedApiVersion, this.OnResourceChanged, value); this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(3)]
        public TGIBlock TRIMRef
        {
            get { return trimRef; }
            set { if (!trimRef.Equals(value)) { trimRef = new TGIBlock(kRecommendedApiVersion, this.OnResourceChanged, TGIBlock.Order.ITG, value.ResourceType, value.ResourceGroup, value.Instance); this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(4)]
        public uint MaterialVariant
        {
            get { return materialVariant; }
            set { if (materialVariant != value) { materialVariant = value; this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(5)]
        public TGIBlock MODLRef
        {
            get { return modlRef; }
            set { if (!modlRef.Equals(value)) { modlRef = new TGIBlock(kRecommendedApiVersion, this.OnResourceChanged, TGIBlock.Order.ITG, value.ResourceType, value.ResourceGroup, value.Instance); this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(6)]
        public ulong SwatchGrouping
        {
            get { return swatchGrouping; }
            set { if (swatchGrouping != value) { swatchGrouping = value; this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(7)]
        public ColorList Colors
        {
            get { return colors; }
            set { if (!colors.Equals( value)) { colors = new ColorList(this.OnResourceChanged, value); this.OnResourceChanged(this, EventArgs.Empty); } }
        }
        [ElementPriority(8)]
        public uint Unk01
        {
            get { return unk01; }
            set { if (unk01 != value) { unk01 = value; this.OnResourceChanged(this, EventArgs.Empty); } }
        }

        public string Value { get { return ValueBuilder; } }

        public override List<string> ContentFields
        {
            get
            {
                List<string> res = base.ContentFields;
                return res;
            }
        }

        #endregion

        #region Data I/O

        void Parse(Stream s)
        {
            var br = new BinaryReader(s);
            this.version = br.ReadUInt32();
            this.commonA = new CatalogCommon(kRecommendedApiVersion, this.OnResourceChanged, s);
            this.trimRef = new TGIBlock(kRecommendedApiVersion, this.OnResourceChanged, TGIBlock.Order.ITG, s);
            this.materialVariant = br.ReadUInt32();
            this.modlRef = new TGIBlock(kRecommendedApiVersion, this.OnResourceChanged, TGIBlock.Order.ITG, s);
            this.swatchGrouping = br.ReadUInt64();
            this.colors = new ColorList(this.OnResourceChanged, s);
            this.unk01 = br.ReadByte();
        }

        protected override Stream UnParse()
        {
            var s = new MemoryStream();
            var bw = new BinaryWriter(s);
            bw.Write(this.version);
            if (this.commonA == null) { this.commonA = new CatalogCommon(kRecommendedApiVersion, this.OnResourceChanged); }
            this.commonA.UnParse(s);
            if (this.trimRef == null) { this.trimRef = new TGIBlock(kRecommendedApiVersion, this.OnResourceChanged, TGIBlock.Order.ITG); }
            this.trimRef.UnParse(s);
            bw.Write(this.materialVariant);
            if (this.modlRef == null) { this.modlRef = new TGIBlock(kRecommendedApiVersion, this.OnResourceChanged, TGIBlock.Order.ITG); }
            this.modlRef.UnParse(s);
            bw.Write(this.swatchGrouping);
            if (this.colors == null) { this.colors = new ColorList(this.OnResourceChanged); }
            this.colors.UnParse(s);
            bw.Write(this.unk01);
            return s;
        }

        #endregion DataIO ===================================================

        #region Constructors
        public CFTRResource(int APIversion, Stream s)
            : base(APIversion, s)
        {
            if (s == null || s.Length == 0)
            {
                s = UnParse();
                OnResourceChanged(this, EventArgs.Empty);
            }
            s.Position = 0;
            this.Parse(s);
        }
        #endregion
    }

    public class CFTRResourceHandler : AResourceHandler
    {
        public CFTRResourceHandler()
        {
            this.Add(typeof(CFTRResource), new List<string>(new[] { "0xE7ADA79D" }));
        }
    }
}