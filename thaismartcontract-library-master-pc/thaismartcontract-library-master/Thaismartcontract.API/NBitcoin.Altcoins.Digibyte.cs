using NBitcoin.DataEncoders;
using System;

namespace NBitcoin.Altcoins
{
	public class Digibyte : NetworkSetBase
	{
		public static Digibyte Instance { get; } = new Digibyte();

		public override string CryptoCode => "DGB";

		private Digibyte()
		{

		}
		//Format visual studio
		//{({.*?}), (.*?)}
		//Tuple.Create(new byte[]$1, $2)
		// Taken from https://github.com/digibyte/digibyte/blob/master/src/chainparamsseeds.h
		static Tuple<byte[], int>[] pnSeed6_main = {
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x05,0xbd,0x98,0x3c}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x18,0x61,0x5e,0xdb}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2d,0x20,0xf2,0xad}, 60051),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2d,0x4c,0xf9,0xd9}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x2f,0x5d,0xa1,0x44}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x34,0x2d,0x58,0x25}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x3a,0xad,0x99,0xa9}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x3c,0x0c,0x7b,0x2a}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x42,0x6c,0x0f,0x05}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x49,0x9c,0x8c,0x9e}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x4e,0x81,0xf1,0x91}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x4f,0x89,0x43,0x8a}, 8817),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x4f,0xac,0xd7,0x44}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x50,0xd3,0xab,0xa6}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x51,0xa9,0xcc,0x15}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x52,0x75,0xa6,0x4d}, 12025),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x55,0xd6,0x44,0x7a}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x63,0xf8,0xeb,0x6c}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x67,0x44,0xfb,0xee}, 35022),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x67,0x44,0xfb,0xef}, 35022),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xd8,0x76,0x2a}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xd8,0x76,0x2b}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x68,0xef,0xe6,0x83}, 28201),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6c,0x34,0xa4,0x12}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6d,0x7b,0x90,0x2d}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x6d,0xe6,0xef,0x33}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x76,0xbe,0x55,0xe8}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x82,0xf0,0x16,0xca}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x90,0x4c,0x67,0x18}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x95,0xd2,0x9d,0x62}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x98,0xba,0x24,0x56}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0x9a,0x10,0x07,0xf2}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xa3,0xac,0x3d,0x47}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xad,0xd4,0xc0,0xd4}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb0,0x6b,0x83,0x2c}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb2,0x3f,0x65,0x1c}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb2,0xdb,0xb0,0x0e}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb9,0x19,0x30,0x24}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xb9,0x75,0x16,0x24}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xbe,0x75,0x85,0xac}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xc1,0x46,0x2f,0x02}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xc1,0x96,0x64,0x42}, 12024),
			Tuple.Create(new byte[]{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xff,0xff,0xd9,0x67,0x20,0xd5}, 12024)
		};
		static Tuple<byte[], int>[] pnSeed6_test = { };

#pragma warning disable CS0618 // Type or member is obsolete
		public class DigibyteConsensusFactory : ConsensusFactory
		{
			private DigibyteConsensusFactory()
			{
			}

			public static DigibyteConsensusFactory Instance { get; } = new DigibyteConsensusFactory();

			public override BlockHeader CreateBlockHeader()
			{
				return new DigibyteBlockHeader();
			}
			public override Block CreateBlock()
			{
				return new DigibyteBlock(new DigibyteBlockHeader());
			}
		}

		public class DigibyteBlockHeader : BlockHeader
		{
			public override uint256 GetPoWHash()
			{
				throw new NotSupportedException("PoW for BitCore BTX is not supported");
			}
		}

		public class DigibyteBlock : Block
		{
			public DigibyteBlock(DigibyteBlockHeader header) : base(header)
			{

			}
			public override ConsensusFactory GetConsensusFactory()
			{
				return DigibyteConsensusFactory.Instance;
			}
		}

#pragma warning restore CS0618 // Type or member is obsolete

		protected override void PostInit()
		{
			RegisterDefaultCookiePath("Digibyte");
		}

		protected override NetworkBuilder CreateMainnet()
		{
			NetworkBuilder builder = new NetworkBuilder();
			builder.SetConsensus(new Consensus()
			{
				//SubsidyHalvingInterval = 840000,
				//MajorityEnforceBlockUpgrade = 750,
				//MajorityRejectBlockOutdated = 950,
				//MajorityWindow = 1000,
				BIP34Hash = new uint256("0xadd8ca420f557f62377ec2be6e6f47b96cf2e68160d58aeb7b73433de834cca0"),
				PowLimit = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
				PowTargetTimespan = TimeSpan.FromSeconds(0.10 * 24 * 60 * 60),
				PowTargetSpacing = TimeSpan.FromSeconds(60),
				PowAllowMinDifficultyBlocks = false,
				PowNoRetargeting = false,
				RuleChangeActivationThreshold = 28224,
				MinerConfirmationWindow = 40320,
				CoinbaseMaturity = 1,
				ConsensusFactory = DigibyteConsensusFactory.Instance
			})
			.SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 30 })
			.SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 63 })
			.SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 128 })
			.SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
			.SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
			.SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("dgb"))
			.SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("dgb"))
			.SetMagic(0xdab6c3fa)
			.SetPort(12024)
			.SetRPCPort(14022)
			.SetName("dgb-main")
			.AddAlias("dgb-mainnet")
			.AddAlias("digibyte-mainnet")
			.AddAlias("digibyte-main")
			.AddDNSSeeds(new[]
			{
				new DNSSeedData("seed1.digibyte.io", "seed1.digibyte.io"),
				new DNSSeedData("seed2.digibyte.io", "seed2.digibyte.io"),
				new DNSSeedData("seed3.digibyte.io", "seed3.digibyte.io"),
				new DNSSeedData("seed.digibyte.io", "seed.digibyte.io"),
				new DNSSeedData("digihash.co", "digihash.co"),
				new DNSSeedData("digiexplorer.info","digiexplorer.info"),
				new DNSSeedData("seed.digibyteprojects.com","seed.digibyteprojects.com")
			})
			.AddSeeds(ToSeed(pnSeed6_main))
			.SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000ad0f7d7518fc1e90ed28bd0e444ccd8e24d94688355705ed2142006b49d9dd726a62d052f0ff0f1e245925000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d01044555534120546f6461793a2031302f4a616e2f323031342c205461726765743a20446174612073746f6c656e2066726f6d20757020746f203131304d20637573746f6d657273ffffffff01401f0000000000000200ac00000000");
			return builder;
		}

		protected override NetworkBuilder CreateTestnet()
		{
            NetworkBuilder builder = new NetworkBuilder();
            builder.SetConsensus(new Consensus()
            {
                //SubsidyHalvingInterval = 840000,
                //MajorityEnforceBlockUpgrade = 750,
                //MajorityRejectBlockOutdated = 950,
                //MajorityWindow = 1000,
                BIP34Hash = new uint256("0xadd8ca420f557f62377ec2be6e6f47b96cf2e68160d58aeb7b73433de834cca0"),
                PowLimit = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
                PowTargetTimespan = TimeSpan.FromSeconds(0.10 * 24 * 60 * 60),
                PowTargetSpacing = TimeSpan.FromSeconds(60),
                PowAllowMinDifficultyBlocks = false,
                PowNoRetargeting = false,
                RuleChangeActivationThreshold = 28224,
                MinerConfirmationWindow = 40320,
                CoinbaseMaturity = 1,
                ConsensusFactory = DigibyteConsensusFactory.Instance
            })
            .SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 30 })
            .SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 63 })
            .SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 128 })
            .SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
            .SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
            .SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("dgb"))
            .SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("dgb"))
            .SetMagic(0xdab6c3fa)
            .SetPort(12024)
            .SetRPCPort(14022)
            .SetName("dgb-test")
            .AddAlias("dgb-testnet")
            .AddAlias("digibyte-testnet")
            .AddAlias("digibyte-test")
            .AddDNSSeeds(new[]
            {
                new DNSSeedData("seed1.digibyte.io", "seed1.digibyte.io"),
                new DNSSeedData("seed2.digibyte.io", "seed2.digibyte.io"),
                new DNSSeedData("seed3.digibyte.io", "seed3.digibyte.io"),
                new DNSSeedData("seed.digibyte.io", "seed.digibyte.io"),
                new DNSSeedData("digihash.co", "digihash.co"),
                new DNSSeedData("digiexplorer.info","digiexplorer.info"),
                new DNSSeedData("seed.digibyteprojects.com","seed.digibyteprojects.com")
            })
            .AddSeeds(ToSeed(pnSeed6_main))
            .SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000ad0f7d7518fc1e90ed28bd0e444ccd8e24d94688355705ed2142006b49d9dd726a62d052f0ff0f1e245925000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d01044555534120546f6461793a2031302f4a616e2f323031342c205461726765743a20446174612073746f6c656e2066726f6d20757020746f203131304d20637573746f6d657273ffffffff01401f0000000000000200ac00000000");
            return builder;
        }

		protected override NetworkBuilder CreateRegtest()
		{
            NetworkBuilder builder = new NetworkBuilder();
            builder.SetConsensus(new Consensus()
            {
                //SubsidyHalvingInterval = 840000,
                //MajorityEnforceBlockUpgrade = 750,
                //MajorityRejectBlockOutdated = 950,
                //MajorityWindow = 1000,
                BIP34Hash = new uint256("0xadd8ca420f557f62377ec2be6e6f47b96cf2e68160d58aeb7b73433de834cca0"),
                PowLimit = new Target(new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff")),
                PowTargetTimespan = TimeSpan.FromSeconds(0.10 * 24 * 60 * 60),
                PowTargetSpacing = TimeSpan.FromSeconds(60),
                PowAllowMinDifficultyBlocks = false,
                PowNoRetargeting = false,
                RuleChangeActivationThreshold = 28224,
                MinerConfirmationWindow = 40320,
                CoinbaseMaturity = 1,
                ConsensusFactory = DigibyteConsensusFactory.Instance
            })
            .SetBase58Bytes(Base58Type.PUBKEY_ADDRESS, new byte[] { 30 })
            .SetBase58Bytes(Base58Type.SCRIPT_ADDRESS, new byte[] { 63 })
            .SetBase58Bytes(Base58Type.SECRET_KEY, new byte[] { 128 })
            .SetBase58Bytes(Base58Type.EXT_PUBLIC_KEY, new byte[] { 0x04, 0x88, 0xB2, 0x1E })
            .SetBase58Bytes(Base58Type.EXT_SECRET_KEY, new byte[] { 0x04, 0x88, 0xAD, 0xE4 })
            .SetBech32(Bech32Type.WITNESS_PUBKEY_ADDRESS, Encoders.Bech32("dgb"))
            .SetBech32(Bech32Type.WITNESS_SCRIPT_ADDRESS, Encoders.Bech32("dgb"))
            .SetMagic(0xdab6c3fa)
            .SetPort(12024)
            .SetRPCPort(14022)
            .SetName("dgb-reg")
            .AddAlias("dgb-regnet")
            .AddAlias("digibyte-regnet")
            .AddAlias("digibyte-reg")
            .AddDNSSeeds(new[]
            {
                new DNSSeedData("seed1.digibyte.io", "seed1.digibyte.io"),
                new DNSSeedData("seed2.digibyte.io", "seed2.digibyte.io"),
                new DNSSeedData("seed3.digibyte.io", "seed3.digibyte.io"),
                new DNSSeedData("seed.digibyte.io", "seed.digibyte.io"),
                new DNSSeedData("digihash.co", "digihash.co"),
                new DNSSeedData("digiexplorer.info","digiexplorer.info"),
                new DNSSeedData("seed.digibyteprojects.com","seed.digibyteprojects.com")
            })
            .AddSeeds(ToSeed(pnSeed6_main))
            .SetGenesis("010000000000000000000000000000000000000000000000000000000000000000000000ad0f7d7518fc1e90ed28bd0e444ccd8e24d94688355705ed2142006b49d9dd726a62d052f0ff0f1e245925000101000000010000000000000000000000000000000000000000000000000000000000000000ffffffff4d04ffff001d01044555534120546f6461793a2031302f4a616e2f323031342c205461726765743a20446174612073746f6c656e2066726f6d20757020746f203131304d20637573746f6d657273ffffffff01401f0000000000000200ac00000000");
            return builder;
        }
	}
}
