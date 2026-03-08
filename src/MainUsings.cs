#pragma warning disable IDE0005
// ReSharper disable RedundantUsingDirective.Global

global using s8 = sbyte;
global using s16 = short;
global using s32 = int;
global using s64 = long;
#if NET7_0_OR_GREATER
global using s128 = System.Int128;
#endif
global using u8 = byte;
global using u16 = ushort;
global using u32 = uint;
global using u64 = ulong;
#if NET7_0_OR_GREATER
global using u128 = System.UInt128;
#endif
global using iLG = System.Numerics.BigInteger;
#if NET5_0_OR_GREATER
global using f16 = System.Half;
#endif
global using f32 = float;
global using f64 = double;
global using f128 = decimal;
