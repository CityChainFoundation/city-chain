using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NBitcoin;
using Stratis.Bitcoin.Features.Wallet.Interfaces;

namespace City.Features.WalletService.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/walletservice")]
    public class WalletServiceController
    {
        private readonly IWalletServiceManager walletServiceManager;
        private readonly IWalletManager walletManager;
        private readonly Network network;

        public WalletServiceController(
            IWalletServiceManager walletServiceManager,
            IWalletManager walletManager,
            Network network)
        {
            this.walletServiceManager = walletServiceManager;
            this.walletManager = walletManager;
            this.network = network;
        }

        /// <summary>
        /// Adds a base58 address to the watch list.
        /// </summary>
        /// <example>Request URL: /api/watchonlywallet/watch?address=mpK6g... </example>
        /// <param name="address">The base58 address to add to the watch list.</param>
        [Route("create")]
        [HttpPost]
        public IActionResult Watch([FromBody]string extPubKey)
        {
            var wallet = this.walletManager.RecoverWallet(Guid.NewGuid().ToString(), ExtPubKey.Parse(extPubKey, this.network), 0, DateTime.UtcNow.AddDays(4));

            return null;
        }
    }
}
