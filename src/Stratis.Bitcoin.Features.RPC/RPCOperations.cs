﻿namespace Stratis.Bitcoin.Features.RPC
{
    //from rpcserver.h
    public enum RPCOperations
    {
        getconnectioncount,
        getpeerinfo,
        ping,
        addnode,
        getaddednodeinfo,
        getnettotals,

        dumpprivkey,
        importprivkey,
        importaddress,
        dumpwallet,
        importwallet,

        getgenerate,
        setgenerate,
        generate,
        generatetoaddress,
        getnetworkhashps,
        gethashespersec,
        getmininginfo,
        prioritisetransaction,
        getwork,
        getblocktemplate,
        submitblock,
        estimatefee,

        getnewaddress,
        getunusedaddress,
        getaccountaddress,
        getrawchangeaddress,
        setaccount,
        getaccount,
        getaddressesbyaccount,
        sendtoaddress,
        signmessage,
        verifymessage,
        getreceivedbyaddress,
        getreceivedbyaccount,
        getbalance,
        getunconfirmedbalance,
        movecmd,
        sendfrom,
        sendmany,
        addmultisigaddress,
        createmultisig,
        listreceivedbyaddress,
        listreceivedbyaccount,
        listtransactions,
        listaddressgroupings,
        listaccounts,
        listsinceblock,
        gettransaction,
        backupwallet,
        keypoolrefill,
        walletpassphrase,
        walletpassphrasechange,
        walletlock,
        encryptwallet,
        validateaddress,
        getinfo,
        getwalletinfo,
        getblockchaininfo,
        getnetworkinfo,

        getrawtransaction,
        listunspent,
        lockunspent,
        listlockunspent,
        createrawtransaction,
        decoderawtransaction,
        decodescript,
        signrawtransaction,
        sendrawtransaction,
        gettxoutproof,
        verifytxoutproof,

        getblockcount,
        getbestblockhash,
        getdifficulty,
        settxfee,
        getmempoolinfo,
        getrawmempool,
        getblockhash,
        getblock,
        gettxoutsetinfo,
        gettxout,
        verifychain,
        getchaintips
    }
}
