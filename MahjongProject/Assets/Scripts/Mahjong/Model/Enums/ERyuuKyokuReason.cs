

public enum ERyuuKyokuReason 
{
    None         = 0,
    NoTsumoHai   = 1,  // no tsumo hai any more, this is normal ryuu kyoku.
    HaiTypeOver9 = 2,  // when any player pick first hai, there are 9 types of IchiKyuu and Tsuu hais in tehai.
    SuteFonHai4  = 3,  // 4 players sute the same Fon hai in the first turn.
    KanOver4     = 4,  // there are 4 kans but they are not belong to one player.
    Reach4       = 5,  // all 4 players Reach.
    Ron3         = 6,  // 3 players Ron at the same time.
}
