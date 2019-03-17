using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using City.Chain.Tests.Features.Wallet;
using City.Networks;
using Moq;
using NBitcoin;
using NBitcoin.Protocol;
using Newtonsoft.Json;
using Stratis.Bitcoin.Configuration;
using Stratis.Bitcoin.Consensus;
using Stratis.Bitcoin.Features.Wallet;
using Stratis.Bitcoin.Features.Wallet.Interfaces;
using Stratis.Bitcoin.Networks;
using Stratis.Bitcoin.Tests.Common.Logging;
using Stratis.Bitcoin.Utilities;
using Xunit;

namespace City.Chain.Tests
{
    public class CompatibilityTests : LogsTestBase, IClassFixture<WalletFixture>
    {
        [Fact]
        public void VerifyBIP38Compatibility()
        {
            var network = CityNetworks.City.Mainnet.Invoke();
            var recoveryPhrase = "praise you muffin lion enable neck grocery crumble super myself license ghost";
            var passphrase = string.Empty;
            var password = "password";

            ExtKey extendedKey = HdOperations.GetExtendedKey(recoveryPhrase, passphrase);
            var secret = extendedKey.PrivateKey.GetEncryptedBitcoinSecret(password, network);
            string encryptedSeed = secret.ToWif();

            Assert.Equal("6PYUdkYbzk9G1Khmf3pRZhcLzqtZJqt9ZnkFjVkYvi76FesXSfH2Q5jTrX", encryptedSeed);
        }

        [Fact]
        [Trait("UnitTest", "UnitTest")]
        public void VerifyCompatibilityWithBitcoinJs()
        {
            var network = CityNetworks.City.Mainnet.Invoke();

            var recoveryPhrase = "praise you muffin lion enable neck grocery crumble super myself license ghost";

            ExtKey masterSeed = HdOperations.GetExtendedKey(recoveryPhrase, string.Empty); // same as new Mnemonic(recoveryPhrase).DeriveExtKey(string.Empty);
            Key privateKey = masterSeed.PrivateKey;
            ExtPubKey extPubKey = HdOperations.GetExtendedPublicKey(privateKey, masterSeed.ChainCode, network.Consensus.CoinType, 0);

            string accountHdPath = HdOperations.GetAccountHdPath(network.Consensus.CoinType, 0);
            var extPubKeyString = extPubKey.GetWif(network).ToString();
            //PubKey pubkey = HdOperations.GeneratePublicKey(extPubKey.GetWif(network).ToString(), 0, false);

            //BitcoinPubKeyAddress address = pubkey.GetAddress(network);

            // Add the new address details to the list of addresses.

            extPubKeyString = "xpub6C4HAsYbVfua5qmGZjhFy31drGYZf5zVpkEXuvthw41bUvPUtBz2ZtNfdB4Majdn7vRTjFHb589NkKFfiwBnZA2Qx4Qu8yHiJG38bHEpTTr";

            int firstNewAddressIndex = 0;
            bool isChange = false;

            List<HdAddress> addresses = new List<HdAddress>();
            for (int i = firstNewAddressIndex; i < firstNewAddressIndex + 3; i++)
            {
                // Generate a new address.
                PubKey pubkey = HdOperations.GeneratePublicKey(extPubKeyString, i, isChange);
                BitcoinPubKeyAddress address = pubkey.GetAddress(network);

                // Add the new address details to the list of addresses.
                var newAddress = new HdAddress
                {
                    Index = i,
                    HdPath = HdOperations.CreateHdPath(network.Consensus.CoinType, 0, isChange, i),
                    ScriptPubKey = address.ScriptPubKey,
                    Pubkey = pubkey.ScriptPubKey,
                    Address = address.ToString(),
                    Transactions = new List<TransactionData>()
                };

                addresses.Add(newAddress);
            }

            //List<HdAddress> addresses = new List<HdAddress>();

            //addresses.Add(new HdAddress
            //{
            //    Index = 0,
            //    HdPath = HdOperations.CreateHdPath(network.Consensus.CoinType, 0, false, 0),
            //    ScriptPubKey = address.ScriptPubKey,
            //    Pubkey = pubkey.ScriptPubKey,
            //    Address = address.ToString(),
            //    Transactions = new List<TransactionData>()
            //});

            //addresses.Add(new HdAddress
            //{
            //    Index = 0,
            //    HdPath = HdOperations.CreateHdPath(network.Consensus.CoinType, 0, false, 1),
            //    ScriptPubKey = address.ScriptPubKey,
            //    Pubkey = pubkey.ScriptPubKey,
            //    Address = address.ToString(),
            //    Transactions = new List<TransactionData>()
            //});

            //addresses.Add(new HdAddress
            //{
            //    Index = 0,
            //    HdPath = HdOperations.CreateHdPath(network.Consensus.CoinType, 0, true, 0),
            //    ScriptPubKey = address.ScriptPubKey,
            //    Pubkey = pubkey.ScriptPubKey,
            //    Address = address.ToString(),
            //    Transactions = new List<TransactionData>()
            //});

            //addresses.Add(new HdAddress
            //{
            //    Index = 0,
            //    HdPath = HdOperations.CreateHdPath(network.Consensus.CoinType, 0, true, 1),
            //    ScriptPubKey = address.ScriptPubKey,
            //    Pubkey = pubkey.ScriptPubKey,
            //    Address = address.ToString(),
            //    Transactions = new List<TransactionData>()
            //});

            var json = JsonConvert.SerializeObject(addresses);
            
            //Key privateKey = HdOperations.DecryptSeed(encryptedSeed, password, network);

            //ExtPubKey accountExtPubKey = HdOperations.GetExtendedPublicKey(privateKey, chainCode, accountHdPath);

            //return new HdAccount
            //{
            //    Index = newAccountIndex,
            //    ExtendedPubKey = accountExtPubKey.ToString(network),
            //    ExternalAddresses = new List<HdAddress>(),
            //    InternalAddresses = new List<HdAddress>(),
            //    Name = newAccountName,
            //    HdPath = accountHdPath,
            //    CreationTime = accountCreationTime
            //};

            //var hd = HdOperations.GetExtendedKey(, string.Empty);
            //var network = CityNetworks.City.Mainnet.Invoke();

            //// Generate a new address.
            //PubKey pubkey = HdOperations.GeneratePublicKey(this.ExtendedPubKey, i, isChange);
            //BitcoinPubKeyAddress address = pubkey.GetAddress(network);

            //// Add the new address details to the list of addresses.
            //var newAddress = new HdAddress
            //{
            //    Index = i,
            //    HdPath = HdOperations.CreateHdPath((int)this.GetCoinType(), this.Index, isChange, i),
            //    ScriptPubKey = address.ScriptPubKey,
            //    Pubkey = pubkey.ScriptPubKey,
            //    Address = address.ToString(),
            //    Transactions = new List<TransactionData>()
            //};

            //addresses.Add(newAddress);
            //addressesCreated.Add(newAddress);


        }
    }
}
