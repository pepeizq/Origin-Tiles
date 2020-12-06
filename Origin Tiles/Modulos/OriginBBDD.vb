Module OriginBBDD

    Public Function BuscarenListado(idOrigin As String)

        Dim lista As New List(Of OriginBBDDEntrada) From {
            New OriginBBDDEntrada("A Way Out", "Origin.OFR.50.0001900", "1222700"),
            New OriginBBDDEntrada("Apex Legends", "Origin.OFR.50.0002694", "1172470"),
            New OriginBBDDEntrada("Anthem Deluxe Edition", "Origin.OFR.50.0002980", Nothing),
            New OriginBBDDEntrada("Battlefield 1", "Origin.OFR.50.0000557", "1238840"),
            New OriginBBDDEntrada("Battlefield 1942", "OFB-EAST:56186", Nothing),
            New OriginBBDDEntrada("Battlefield 2 Complete Collection", "DR78869400", Nothing),
            New OriginBBDDEntrada("Battlefield 3", "DR:225064100", "1238820"),
            New OriginBBDDEntrada("Battlefield 3 Limited Edition", "DR:224766400", "1238820"),
            New OriginBBDDEntrada("Battlefield 4 Premium Edition", "OFB-EAST:109552316", "1238860"),
            New OriginBBDDEntrada("Battlefield Bad Company 2", "DR:156691300", "24960"),
            New OriginBBDDEntrada("Battlefield Hardline Ultimate Edition", "Origin.OFR.50.0000846", "1238880"),
            New OriginBBDDEntrada("Battlefield V Definitive Edition", "Origin.OFR.50.0002942", "1238810"),
            New OriginBBDDEntrada("Bejeweled 3", "OFB-EAST:49753", "78000"),
            New OriginBBDDEntrada("Burnout Paradise Remastered", "Origin.OFR.50.0002535", "1238080"),
            New OriginBBDDEntrada("Burnout Paradise The Ultimate Box", "DR:106479900", "24740"),
            New OriginBBDDEntrada("Command & Conquer", "OFB-EAST:52016", Nothing),
            New OriginBBDDEntrada("Command & Conquer 3: Tiberium Wars", "OFB-EAST:52210", "24790"),
            New OriginBBDDEntrada("Command & Conquer 4: Tiberian Twilight", "OFB-EAST:52212", "47700"),
            New OriginBBDDEntrada("Command & Conquer Generals", "OFB-EAST:52209", Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert", "OFB-EAST:52017", Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert 2", "OFB-EAST:52019", Nothing),
            New OriginBBDDEntrada("Command & Conquer Red Alert 3", "OFB-EAST:52211", "17480"),
            New OriginBBDDEntrada("Command & Conquer Red Alert 3 Uprising", "DR:111298000", "24800"),
            New OriginBBDDEntrada("Command & Conquer Remastered Collection", "Origin.OFR.50.0004022", "1213210"),
            New OriginBBDDEntrada("Command & Conquer Renegade", "OFB-EAST:52208", Nothing),
            New OriginBBDDEntrada("Command & Conquer Tiberian Sun", "OFB-EAST:52018", Nothing),
            New OriginBBDDEntrada("Crysis", "DR:77033800", "17300"),
            New OriginBBDDEntrada("Crysis 2", "OFB-EAST:49464", "108800"),
            New OriginBBDDEntrada("Crysis 3", "OFB-EAST:49562", "1282690"),
            New OriginBBDDEntrada("Crysis Warhead", "DR:105905500", "17330"),
            New OriginBBDDEntrada("Dead Space", "OFB-EAST:109547518", "17470"),
            New OriginBBDDEntrada("Dead Space 2", "OFB-EAST:55619", "47780"),
            New OriginBBDDEntrada("Dead Space 3", "OFB-EAST:50885", "1238060"),
            New OriginBBDDEntrada("Dragon Age II", "DR:201797000", "1238040"),
            New OriginBBDDEntrada("Dragon Age Inquisition", "Origin.OFR.50.0001131", "1222690"),
            New OriginBBDDEntrada("Dragon Age Origins Ultimate Edition", "DR:208591800", "47810"),
            New OriginBBDDEntrada("Dungeon Keeper", "DR:231813000", Nothing),
            New OriginBBDDEntrada("Dungeon Keeper 2", "OFB-EAST:39131", Nothing),
            New OriginBBDDEntrada("Fe", "Origin.OFR.50.0001944", "1225580"),
            New OriginBBDDEntrada("FIFA 20 Ultimate Edition", "Origin.OFR.50.0003453", Nothing),
            New OriginBBDDEntrada("FIFA 21 Ultimate Edition", "Origin.OFR.50.0004195", "1313860"),
            New OriginBBDDEntrada("Jade Empire: Special Edition", "OFB-EAST:1000017", "7110"),
            New OriginBBDDEntrada("Madden NFL 20 Ultimate Edition", "Origin.OFR.50.0003417", Nothing),
            New OriginBBDDEntrada("Madden NFL 21 Ultimate Edition", "Origin.OFR.50.0004125", "1239520"),
            New OriginBBDDEntrada("Mass Effect", "DR:102427200", "17460"),
            New OriginBBDDEntrada("Mass Effect 2", "OFB-EAST:56694", "24980"),
            New OriginBBDDEntrada("Mass Effect 3", "DR:229644400", "1238020"),
            New OriginBBDDEntrada("Mass Effect Andromeda", "Origin.OFR.50.0001539", "1238000"),
            New OriginBBDDEntrada("Medal of Honor", "DR:215619100", "24840"),
            New OriginBBDDEntrada("Medal of Honor: Airbone", "DR:80586700", "47790"),
            New OriginBBDDEntrada("Medal of Honor: Allied Assault", "OFB-EAST:1000011", Nothing),
            New OriginBBDDEntrada("Medal of Honor: Pacific Assault", "Origin.OFR.50.0000357", Nothing),
            New OriginBBDDEntrada("Mirror's Edge", "DR:106999100", "17410"),
            New OriginBBDDEntrada("Mirror's Edge Catalyst", "Origin.OFR.50.0001000", "1233570"),
            New OriginBBDDEntrada("Need For Speed Deluxe Edition", "Origin.OFR.50.0001009", "1262540"),
            New OriginBBDDEntrada("Need For Speed Heat Deluxe Edition", "Origin.OFR.50.0003514", "1222680"),
            New OriginBBDDEntrada("Need For Speed Hot Pursuit", "DR:210703600", Nothing),
            New OriginBBDDEntrada("Need For Speed Hot Pursuit Remastered", "Origin.OFR.50.0004030", "1328660"),
            New OriginBBDDEntrada("Need For Speed Payback Deluxe Edition", "Origin.OFR.50.0002167", "1262580"),
            New OriginBBDDEntrada("Need For Speed Rivals Complete Edition", "Origin.OFR.50.0000676", "1262600"),
            New OriginBBDDEntrada("Peggle Deluxe", "Origin.OFR.50.0000461", "3480"),
            New OriginBBDDEntrada("Plants vs. Zombies: Battle for Neighborville Deluxe Edition", "Origin.OFR.50.0003675", "1262240"),
            New OriginBBDDEntrada("Plants vs. Zombies: Game of the Year", "OFB-EAST:48217", "3590"),
            New OriginBBDDEntrada("Plants vs. Zombies: Garden Warfare 2 Deluxe Edition", "Origin.OFR.50.0001051", Nothing),
            New OriginBBDDEntrada("Rocket Arena", "Origin.OFR.50.0003997", "1233550"),
            New OriginBBDDEntrada("Sea of Solitude", "Origin.OFR.50.0002405", "1225590"),
            New OriginBBDDEntrada("Shank", "DR:224434500", "6120"),
            New OriginBBDDEntrada("SimCity 2000 Special Edition", "DR:235664600", Nothing),
            New OriginBBDDEntrada("SimCity 4 Deluxe Edition", "DR:198095500", "24780"),
            New OriginBBDDEntrada("SimCity Limited Edition", "OFB-EAST:48205", Nothing),
            New OriginBBDDEntrada("Spore", "DR:91619200", "17390"),
            New OriginBBDDEntrada("Star Wars Battlefront", "Origin.OFR.50.0001211", "1237980"),
            New OriginBBDDEntrada("Star Wars Battlefront II", "Origin.OFR.50.0002015", "1237950"),
            New OriginBBDDEntrada("Star Wars Jedi: Fallen Order Deluxe Edition", "Origin.OFR.50.0003815", "1172380"),
            New OriginBBDDEntrada("Star Wars Squadrons", "Origin.OFR.50.0004099", "1222730"),
            New OriginBBDDEntrada("Star Wars The Old Republic", "DR:231255400", "1286830"),
            New OriginBBDDEntrada("The Saboteur", "DR:106999600", Nothing),
            New OriginBBDDEntrada("The Sims 3", "OFB-EAST:55107", "47890"),
            New OriginBBDDEntrada("The Sims 4", "OFB-EAST:109552299", "1222670"),
            New OriginBBDDEntrada("Theme Hospital", "Origin.OFR.50.0000500", Nothing),
            New OriginBBDDEntrada("Titanfall Deluxe Edition", "Origin.OFR.50.0000739", "1454890"),
            New OriginBBDDEntrada("Titanfall 2 Deluxe Edition", "Origin.OFR.50.0001456", "1237970"),
            New OriginBBDDEntrada("Unravel", "Origin.OFR.50.0000823", "1225560"),
            New OriginBBDDEntrada("Unravel Two", "Origin.OFR.50.0002403", "1225570")
        }

        Dim idSteam As String = String.Empty

        For Each juego In lista
            If idOrigin = juego.IDOrigin Then
                idSteam = juego.IDSteam
                Exit For
            End If
        Next

        Return idSteam

    End Function

    Public Class OriginBBDDEntrada

        Public Titulo As String
        Public IDOrigin As String
        Public IDSteam As String

        Public Sub New(ByVal titulo As String, ByVal idOrigin As String, ByVal idSteam As String)
            Me.Titulo = titulo
            Me.IDOrigin = idOrigin
            Me.IDSteam = idSteam
        End Sub

    End Class

End Module
