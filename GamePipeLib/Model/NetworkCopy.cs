﻿/**
* Copyright (c) 2016 Joseph Shaw
* Distributed under the GNU GPL v2. For full terms see the file LICENSE.txt
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamePipeLib.Interfaces;
using System.IO;

namespace GamePipeLib.Model
{

    public class NetworkCopy : TransferBase
    {
        public NetworkCopy(IAppProvider source, ITransferTarget target, ISteamApplication app) : base(source, target, app) { }
        public override string TransferType { get { return "Network Copy"; } }
        public override bool CanPauseMidStream() { return false; }

        protected override void DoAbortProcess()
        {
            //Nothing to do
        }

        protected override void DoPostProcess()
        {
            string appId = Application.AppId;
            _target.WriteAcfFile(appId, _source.GetAcfFileContent(appId));
        }

        protected override void DoPreProcess()
        {
            //Nothing to do
        }

        protected override bool ValidateFile(string file, Stream source, Stream target)
        {
            string appId = Application.AppId;

            if (target is Utils.CrcStream)
            {
                uint sourceCrc = 0;
                try
                {
                    sourceCrc = _source.GetTransferredCrc(appId, file);
                }
                catch (Exception) { }


                var targetCrc = ((Utils.CrcStream)target).WriteCrc;
                return (targetCrc == sourceCrc);
            }
            else
                return true; //If the target isn't a crc stream then just return true
        }
    }

}