﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Pundit.WindowsService
{
   public partial class PunditService : ServiceBase
   {
      public PunditService()
      {
         InitializeComponent();
      }

      protected override void OnStart(string[] args)
      {
      }

      protected override void OnStop()
      {
      }

      private void StartService()
      {
         
      }
   }
}