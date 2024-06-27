Imports TSOnline.Entities
Imports System.Web.ModelBinding
Imports TSOnline.UI.Web.UIHelper
Imports TSOnline.Business
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Globalization
Imports System.Transactions


Public Class Stok_DaftarHrgAngkut
    Inherits BasePage

    Public Overrides Property PAGE_NAME As String = "/Stok/DaftarHrgAngkut"

    Dim MDComp As New MasterDataComponent
    Dim GComp = New TSOnline.Business.GeneralComponent
    Dim AGComp As New AngkutComponent

    Dim DC_HgAngkt As DC_HrgAngkutan
    Dim DC_Pdlmn As DC_Pedalaman
    Dim DC_StokItem As DC_StokItem
    Dim AuditTrail As New Audit_Trail

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If (Not Page.IsPostBack) Then

            Dim statuspage As String = Request.QueryString("Status")


            If statuspage = "Edit1" Or statuspage = "View" Then

                If (Not String.IsNullOrEmpty(Request.QueryString("NoSiri_Hrg")) And Not String.IsNullOrEmpty(Request.QueryString("Status"))) Then

                    DC_HgAngkt = MDComp.getdetailhrgangkt(Request.QueryString("NoSiri_Hrg"))

                    If (DC_HgAngkt.Status = "LULUS") Then

                        If Permissions.Lulus Then
                            GetInfoHrgAngkut()
                        Else
                            GetInfoHrgAngkut()
                            enabledisabled()
                        End If

                    Else

                        GetInfoHrgAngkut()
                        buttoncontrol()

                    End If


                    If (DC_HgAngkt.Status = "LULUS") Then
                        btnSimpan.Visible = False
                        btnLulus.Visible = False
                        If Permissions.Lulus Then
                            txttkhefektif.Enabled = True
                            txthrgangkutBJ_10.Enabled = True
                            txthrgangkutBJ_25.Enabled = True
                            txthrgangkutBH.Text = True
                        End If
                    End If

                Else

                    'DC_Pdlmn = MDComp.getdetailhrgangkt_Pdlmn(Request.QueryString("TujuanAngkut"), Request.QueryString("KodHantar"), Request.QueryString("U_Negeri_ID"), Request.QueryString("Kod_Daerah"), Request.QueryString("Kod_Mukim"), Request.QueryString("Kod_Kampung"))

                    DC_Pdlmn = MDComp.getdetailhrgangkt_Pdlmn(Request.QueryString("Pedalaman_ID"))


                    If (DC_Pdlmn.Status = "LULUS") Then

                        If Permissions.Lulus Then
                            GetInfoHrgAngkut()
                        Else
                            GetInfoHrgAngkut()
                            enabledisabled()
                        End If

                    Else

                        GetInfoHrgAngkut()
                        buttoncontrol()

                    End If


                    If (DC_Pdlmn.Status = "LULUS") Then
                        btnSimpan.Visible = False
                        btnLulus.Visible = False

                        If Permissions.Lulus Then
                            txttkhefektif.Enabled = True
                            txtkosbj10.Enabled = True
                            txtkosobj25.Enabled = True
                            txtkosbenih.Enabled = True
                        End If

                    End If

                End If



            Else

                Dim kod_pt As String = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.Kod_PT

                hdnMode.Value = "Add"

                txt_tkhkemaskini.Text = DateTime.Now.ToString("dd-MM-yyyy")
                txtPeg_Kemaskini.Text = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.UserName

                ddl_kg.Enabled = False


                buttoncontrol()

            End If

            If statuspage = "View" Then
                enabledisabled()
            End If

            If Permissions.Lulus Then
                btnSimpan.Visible = False
            End If

            setPermissions()

        End If


    End Sub

    Private Sub setPermissions()


        If (btnSimpan.Visible) Then btnSimpan.Visible = Permissions.Tambah
        If (btnLulus.Visible) Then btnLulus.Visible = Permissions.Lulus

        If (btnSimpan.Visible) Then
            If Not (Permissions.Tambah) Then
                Response.Redirect("~/default")
            End If
        End If

        If (Not Page.IsPostBack) Then
            If (Not String.IsNullOrWhiteSpace(txtPeg_Kemaskini.Text)) Then
                txtPeg_Kemaskini.Text = UserManagementComponent.GetUserNameByNoKP(txtPeg_Kemaskini.Text)
            End If
            If (Not String.IsNullOrWhiteSpace(txtPeg_Lulus.Text)) Then
                txtPeg_Lulus.Text = UserManagementComponent.GetUserNameByNoKP(txtPeg_Lulus.Text)
            End If
        End If

    End Sub

    Public Sub buttoncontrol()

        Dim statuspage As String = Request.QueryString("Status")
        Dim DC_HgAngkt_ As DC_HrgAngkutan
        Dim DC_Pdlmn_ As DC_Pedalaman

        If (Not String.IsNullOrEmpty(Request.QueryString("NoSiri_Hrg")) And Not String.IsNullOrEmpty(Request.QueryString("Status"))) Then
            DC_HgAngkt_ = MDComp.getdetailhrgangkt(Request.QueryString("NoSiri_Hrg"))

            If Permissions.Tambah Then

                If statuspage = "View" Then

                    If Trim(DC_HgAngkt_.Status) = "BARU" Then
                        btnKemaskini.Visible = False
                    ElseIf Trim(DC_HgAngkt_.Status) = "LULUS" Then

                    End If

                ElseIf statuspage = "Edit1" Then

                    If Trim(DC_HgAngkt_.Status) = "BARU" Then

                        btnSimpan.Visible = True
                        btnKemaskini.Visible = False

                    ElseIf Trim(DC_HgAngkt_.Status) = "LULUS" Then

                    End If

                Else
                    btnSimpan.Enabled = True
                End If

            ElseIf Permissions.Lulus Then


                If statuspage = "View" Then
                    If Trim(DC_HgAngkt_.Status) = "BARU" Then

                        btnLulus.Visible = True
                        btnKemaskini.Visible = False

                    ElseIf Trim(DC_HgAngkt_.Status) = "LULUS" Then

                    End If

                ElseIf statuspage = "Edit1" Then

                    If Trim(DC_HgAngkt_.Status) = "BARU" Then

                        btnLulus.Visible = True
                        btnSimpan.Enabled = False
                        btnKemaskini.Visible = False

                    ElseIf Trim(DC_HgAngkt_.Status) = "LULUS" Then

                    End If

                End If

            End If

        Else
            DC_Pdlmn_ = MDComp.getdetailhrgangkt_Pdlmn(Request.QueryString("Pedalaman_ID"))

            If Permissions.Tambah Then

                If statuspage = "View" Then

                    If Trim(DC_Pdlmn_.Status) = "BARU" Then
                        btnKemaskini.Visible = False
                    ElseIf Trim(DC_Pdlmn_.Status) = "LULUS" Then

                    End If

                ElseIf statuspage = "Edit1" Then

                    If Trim(DC_Pdlmn_.Status) = "BARU" Then

                        btnSimpan.Visible = True
                        btnKemaskini.Visible = False

                    ElseIf Trim(DC_Pdlmn_.Status) = "LULUS" Then

                    End If

                Else
                    btnSimpan.Enabled = True
                End If

            ElseIf Permissions.Lulus Then


                If statuspage = "View" Then
                    If Trim(DC_Pdlmn_.Status) = "BARU" Then

                        btnLulus.Visible = True
                        btnKemaskini.Visible = False

                    ElseIf Trim(DC_Pdlmn_.Status) = "LULUS" Then

                    End If

                ElseIf statuspage = "Edit1" Then

                    If Trim(DC_Pdlmn_.Status) = "BARU" Then

                        btnLulus.Visible = True
                        btnSimpan.Enabled = False
                        btnKemaskini.Visible = False

                    ElseIf Trim(DC_Pdlmn_.Status) = "LULUS" Then

                    End If

                End If

            End If

        End If

        'If Not statuspage Is Nothing Then
        '    btnLulus.Visible = False
        '    'btnSimpan.Visible = False
        'Else

        'End If



    End Sub

    Public Sub enabledisabled()

        ddljnsangkut.Enabled = False
        ddllokasi.Enabled = False
        ddlphntar.Enabled = False
        ddlpnerima.Enabled = False
        ddljnsstok.Enabled = False
        txthrgangkutBJ_10.Enabled = False
        txthrgangkutBJ_25.Enabled = False
        txthrgangkutBH.Enabled = False
        txtkosbj10.Enabled = False
        txtkosobj25.Enabled = False
        txtkosbenih.Enabled = False
        txttkhefektif.Enabled = False
        txtPeg_Kemaskini.Enabled = False
        txtPeg_Lulus.Enabled = False
        txt_tkhkemaskini.Enabled = False
        txtTkh_Lulus.Enabled = False

        ddl_negeri.Enabled = False
        ddl_daerah.Enabled = False
        ddl_mukim.Enabled = False
        ddl_kg.Enabled = False



    End Sub

    Private Sub GetInfoHrgAngkut()

        Dim usg = New TSOnline.Business.GeneralComponent()
        Dim DS_datas As DataSet

        If (Not String.IsNullOrEmpty(Request.QueryString("NoSiri_Hrg"))) Then
            DC_HgAngkt = MDComp.getdetailhrgangkt(Request.QueryString("NoSiri_Hrg"))

            txtnosirihrg.Text = DC_HgAngkt.NoSiri_Hrg


            ddljnsangkut.SelectedValue = Trim(DC_HgAngkt.TujuanAngkut)

            ddllokasi.SelectedValue = DC_HgAngkt.Lokasi

            ddljnsangkut.SelectedValue = Trim(DC_HgAngkt.TujuanAngkut)
            ddljnsstok.SelectedValue = DC_HgAngkt.JenisStok


            Dim tarikh_efektif As DateTime
            tarikh_efektif = DateTime.Parse(DC_HgAngkt.Tarikh_Efektif)
            txttkhefektif.Text = tarikh_efektif.ToString("dd-MM-yyyy")


            If Trim(DC_HgAngkt.Status) = "LULUS" Then

                txtPeg_Kemaskini.Text = DC_HgAngkt.Peg_Kemaskini
                'txt_tkhkemaskini.Text = DC_HgAngkt.Tkh_Kemaskini

                Dim tarikh_input As DateTime
                tarikh_input = DateTime.Parse(DC_HgAngkt.Tkh_Kemaskini)
                txt_tkhkemaskini.Text = tarikh_input.ToString("dd-MM-yyyy")

                txtPeg_Lulus.Text = DC_HgAngkt.Peg_Lulus
                'txtTkh_Lulus.Text = String.Format("{0:d}", DC_HgAngkt.Tkh_Lulus)

                Dim tarikh_lulus As DateTime
                tarikh_lulus = DateTime.Parse(DC_HgAngkt.Tkh_Lulus)
                txtTkh_Lulus.Text = tarikh_lulus.ToString("dd-MM-yyyy")

                btnLulus.Visible = False
                btnSimpan.Visible = False
                enabledisabled()

            Else

                txtPeg_Kemaskini.Text = DC_HgAngkt.Peg_Kemaskini

                Dim tarikh_input As DateTime
                tarikh_input = DateTime.Parse(DC_HgAngkt.Tkh_Kemaskini)
                'txt_tkhkemaskini.Text = tarikh_input.ToString("dd-MM-yyyy")

                'Dim tarikh_input As DateTime
                tarikh_input = DateTime.Parse(DC_HgAngkt.Tkh_Kemaskini)
                txt_tkhkemaskini.Text = tarikh_input.ToString("dd-MM-yyyy")

            End If


            If ddljnsangkut.SelectedValue = "Kilang–Stor" Then

                SetKilangSPUR()

                ddlphntar.SelectedValue = DC_HgAngkt.KodHantar.Trim
                ddlpnerima.SelectedValue = Trim(DC_HgAngkt.KodTerima)

                If DC_HgAngkt.Ukuran.Trim = "10" Then
                    lblukuran.Text = "Harga Angkut Baja 10kg (RM)"
                ElseIf DC_HgAngkt.Ukuran.trim = "25" Then
                    lblukuran.Text = "Harga Angkut Baja 25kg (RM)"
                End If
                txthrgangkutBJ_10.Text = DC_HgAngkt.HrgAngkut

                txthrgangkutBJ_25.Text = "0.00"

                txthrgangkutBH.Text = "0.00"
                txthrgangkutBH.Visible = False
                hgaagktbh.Visible = False

                rdaktif.Checked = True

            ElseIf ddljnsangkut.SelectedValue = "SP-PT" Then

                SETSPURPT()

                ddlphntar.SelectedValue = Trim(DC_HgAngkt.KodHantar)
                ddlpnerima.SelectedValue = Trim(DC_HgAngkt.KodTerima)

                txthrgangkutBJ_10.Text = DC_HgAngkt.HrgAngkut

                txthrgangkutBJ_25.Text = "0.00"

                txthrgangkutBH.Text = "0.00"
                txthrgangkutBH.Visible = False
                hgaagktbh.Visible = False

                rdaktif.Checked = True

            ElseIf ddljnsangkut.SelectedValue = "PT-PT" Then

                SetPTPT()

                ddlphntar.SelectedValue = Trim(DC_HgAngkt.KodHantar)
                ddlpnerima.SelectedValue = Trim(DC_HgAngkt.KodTerima)

                txthrgangkutBJ_10.Text = DC_HgAngkt.HrgAngkut

                txthrgangkutBJ_25.Text = "0.00"

                txthrgangkutBH.Text = "0.00"
                txthrgangkutBH.Visible = False
                hgaagktbh.Visible = False

                rdaktif.Checked = True

            ElseIf ddljnsangkut.SelectedValue = "SPT-PK" Or ddljnsangkut.SelectedValue = "Pedalaman" Then

                If ddljnsangkut.SelectedValue = "SPT-PK" Then
                    rdaktif.Checked = True
                End If

                If Trim(DC_HgAngkt.Lokasi) = "Semenanjung" Then
                    BindPTSem()
                    ddlphntar.SelectedValue = Trim(DC_HgAngkt.KodHantar)

                    ddlpnerima.Visible = False
                    pnerima.Visible = False

                    DS_datas = MDComp.getdetailbyKodHtr(ddlphntar.SelectedValue)

                    If (DS_datas.Tables.Count > 0) Then

                        If DS_datas.Tables(0).Rows.Count > 0 Then

                            Dim dtAccounts As DataTable = DS_datas.Tables(0)

                            txthrgangkutBJ_25.Text = "0.00"

                            For Each row As DataRow In dtAccounts.Rows
                                txthrgangkutBJ_10.Text = dtAccounts.Rows(0)("HrgAngkut").ToString
                                txthrgangkutBH.Text = dtAccounts.Rows(1)("HrgAngkut").ToString
                                If DC_HgAngkt.JenisStok = "BJ" Then
                                    txthrgangkutBH.Enabled = False
                                Else
                                    txthrgangkutBJ_10.Enabled = False
                                    txthrgangkutBJ_25.Enabled = False
                                End If
                            Next


                            ddlpnerima.Visible = False
                            pnerima.Visible = False


                        End If

                    End If

                Else

                    If ddllokasi.SelectedValue = "Sabah" Then
                        BindPTSabah()
                    ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                        BindPTSarawak()
                    End If

                    ddlphntar.SelectedValue = Trim(DC_HgAngkt.KodHantar)

                    ddlpnerima.Visible = False
                    pnerima.Visible = False

                    DS_datas = MDComp.getdetailbyKodHtr(ddlphntar.SelectedValue)

                    If (DS_datas.Tables.Count > 0) Then

                        If DS_datas.Tables(0).Rows.Count > 0 Then

                            Dim dtAccounts As DataTable = DS_datas.Tables(0)

                            txthrgangkutBJ_25.Text = "0.00"

                            For Each row As DataRow In dtAccounts.Rows
                                txthrgangkutBJ_10.Text = dtAccounts.Rows(0)("HrgAngkut").ToString
                                txthrgangkutBH.Text = dtAccounts.Rows(1)("HrgAngkut").ToString
                                If DC_HgAngkt.JenisStok = "BJ" Then
                                    txthrgangkutBH.Enabled = False
                                Else
                                    txthrgangkutBJ_10.Enabled = False
                                    txthrgangkutBJ_25.Enabled = False
                                End If
                            Next


                            ddlpnerima.Visible = False
                            pnerima.Visible = False


                        End If

                    End If

                End If


            End If


        Else
            'DC_Pdlmn = MDComp.getdetailhrgangkt_Pdlmn(Request.QueryString("TujuanAngkut"), Request.QueryString("KodHantar"), Request.QueryString("U_Negeri_ID"), Request.QueryString("Kod_Daerah"), Request.QueryString("Kod_Mukim"), Request.QueryString("Kod_Kampung"))
            DC_Pdlmn = MDComp.getdetailhrgangkt_Pdlmn(Request.QueryString("Pedalaman_ID"))


            ddljnsangkut.SelectedValue = Trim(DC_Pdlmn.TujuanAngkut)

            ddllokasi.SelectedValue = DC_Pdlmn.Lokasi

            ddljnsangkut.SelectedValue = DC_Pdlmn.TujuanAngkut

            Dim Tkh_Efektif As DateTime
            Tkh_Efektif = DateTime.Parse(DC_Pdlmn.Tkh_Efektif)
            txttkhefektif.Text = Tkh_Efektif.ToString("dd-MM-yyyy")

            If DC_Pdlmn.StatusAktif.Trim = "AKTIF" Then
                rdaktif.Checked = True
            Else
                rdxaktif.Checked = True
            End If

            If Trim(DC_Pdlmn.Status) = "LULUS" Then

                txtPeg_Kemaskini.Text = DC_Pdlmn.Peg_Input
                txt_tkhkemaskini.Text = DC_Pdlmn.Tkh_Input

                txtPeg_Lulus.Text = DC_Pdlmn.Peg_Lulus
                txtTkh_Lulus.Text = String.Format("{0:d}", DC_Pdlmn.Tkh_Lulus)

                btnLulus.Visible = False
                btnSimpan.Visible = False
                enabledisabled()

            Else

                txtPeg_Kemaskini.Text = DC_Pdlmn.Peg_Input

                Dim Tkh_Input As DateTime
                Tkh_Input = DateTime.Parse(DC_Pdlmn.Tkh_Input)
                txt_tkhkemaskini.Text = Tkh_Input.ToString("dd-MM-yyyy")

            End If

            'ddljnsstok.SelectedValue = DC_Pdlmn.JenisStok

            'If ddljnsangkut.SelectedValue = "Pedalaman" Then

            cbKG.Checked = True

            If Trim(DC_Pdlmn.Lokasi) = "Semenanjung" Then
                BindPTSem()
                ddlphntar.SelectedValue = Trim(DC_Pdlmn.KodHantar)

                ddlpnerima.Visible = False
                pnerima.Visible = False

                DS_datas = MDComp.getdetailbyKodHtr(ddlphntar.SelectedValue)

                If Trim(DC_Pdlmn.Lokasi) = "Semenanjung" Then
                    BindPTSem()
                    ddlphntar.SelectedValue = Trim(DC_Pdlmn.KodHantar)

                    ddlpnerima.Visible = False
                    pnerima.Visible = False

                    'DS_datas = MDComp.getdetailbyKodHtr(ddlphntar.SelectedValue)

                    'If (DS_datas.Tables.Count > 0) Then

                    '    If DS_datas.Tables(0).Rows.Count > 0 Then

                    '        Dim dtAccounts As DataTable = DS_datas.Tables(0)

                    '        txthrgangkutBJ_25.Text = "0.00"

                    '        For Each row As DataRow In dtAccounts.Rows
                    '            txthrgangkutBJ_10.Text = dtAccounts.Rows(0)("HrgAngkut").ToString
                    '            txthrgangkutBH.Text = dtAccounts.Rows(1)("HrgAngkut").ToString
                    '            If DC_HgAngkt.JenisStok = "BJ" Then
                    '                txthrgangkutBH.Enabled = False
                    '            Else
                    '                txthrgangkutBJ_10.Enabled = False
                    '                txthrgangkutBJ_25.Enabled = False
                    '            End If
                    '        Next


                    '        ddlpnerima.Visible = False
                    '        pnerima.Visible = False


                    '    End If

                    'End If
                End If


            Else

                If ddllokasi.SelectedValue = "Sabah" Then
                    BindPTSabah()
                ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                    BindPTSarawak()
                End If

                ddlphntar.SelectedValue = Trim(DC_Pdlmn.KodHantar)

                ddlpnerima.Visible = False
                pnerima.Visible = False

                'DS_datas = MDComp.getdetailbyKodHtr(ddlphntar.SelectedValue)

                'If (DS_datas.Tables.Count > 0) Then

                '    If DS_datas.Tables(0).Rows.Count > 0 Then

                '        Dim dtAccounts As DataTable = DS_datas.Tables(0)

                '        txthrgangkutBJ_25.Text = "0.00"

                '        For Each row As DataRow In dtAccounts.Rows
                '            txthrgangkutBJ_10.Text = dtAccounts.Rows(0)("HrgAngkut").ToString
                '            txthrgangkutBH.Text = dtAccounts.Rows(1)("HrgAngkut").ToString
                '            If DC_HgAngkt.JenisStok = "BJ" Then
                '                txthrgangkutBH.Enabled = False
                '            Else
                '                txthrgangkutBJ_10.Enabled = False
                '                txthrgangkutBJ_25.Enabled = False
                '            End If
                '        Next


                '        ddlpnerima.Visible = False
                '        pnerima.Visible = False


                '    End If

                'End If

            End If

            txtkosbj10.Text = DC_Pdlmn.Kos_10kg
            txtkosobj25.Text = DC_Pdlmn.Kos_25kg
            txtkosbenih.Text = DC_Pdlmn.Kos_Benih

            GetDictStokItem()

            Bindddl_negeri()
            If (Not DC_Pdlmn.U_Negeri_ID Is Nothing) Then
                ddl_negeri.SelectedValue = DC_Pdlmn.U_Negeri_ID.Trim
                Me.ddl_negeri_SelectedIndexChanged(Me.ddl_negeri, System.EventArgs.Empty)
            End If

            If (Not DC_Pdlmn.Kod_Daerah Is Nothing) Then
                ddl_daerah.SelectedValue = DC_Pdlmn.Kod_Daerah.Trim
                Me.ddl_daerah_SelectedIndexChanged(Me.ddl_daerah, System.EventArgs.Empty)
            End If

            If (Not DC_Pdlmn.Kod_Mukim) Then
                ddl_mukim.SelectedValue = DC_Pdlmn.Kod_Mukim.Trim
                Me.ddl_mukim_SelectedIndexChanged(Me.ddl_mukim, System.EventArgs.Empty)
            End If

            If (Not DC_Pdlmn.Kod_Kampung Is Nothing) Then
                ddl_kg.SelectedValue = DC_Pdlmn.Kod_Kampung.Trim
            End If

        End If


        'get harga angkut BJ dan BH biasa per jenisitemstok
        '---------------------------------------------------------
        'Dim jenisstok As String
        'Dim ukuran As String

        'For i As Integer = 1 To 2
        '    If i = 1 Then
        '        jenisstok = "BJ"

        '        ukuran = "10"

        '        Dim datas As DC_HrgAngkutan = MDComp.getdetailhrgangkt_byjenis(ddljnsangkut.SelectedValue, jenisstok, ukuran, ddlphntar.SelectedValue)

        '        txthrgangkutBJ_10.Text = datas.HrgAngkut
        '        txthrgangkutBJ_25.Text = "0.00"


        '    ElseIf i = 2 Then

        '        jenisstok = "BH"
        '        ukuran = "POKOK"
        '        Dim datas As DC_HrgAngkutan = MDComp.getdetailhrgangkt_byjenis(ddljnsangkut.SelectedValue, jenisstok, ukuran, ddlphntar.SelectedValue)

        '        txthrgangkutBH.Text = datas.HrgAngkut

        '    End If
        'Next

        'If ddllokasi.SelectedValue <> "Semenanjung" Then
        '    jenisstok = "BJ"
        '    ukuran = "25"

        '    Dim data As DC_HrgAngkutan = MDComp.getdetailhrgangkt_byjenis(ddljnsangkut.SelectedValue, jenisstok, ukuran, ddlphntar.SelectedValue)

        '    txthrgangkutBJ_25.Text = data.HrgAngkut

        'End If


        'Dim DS_data As DataSet = MDComp.getdetailbyKodHtr(ddlphntar.SelectedValue)

        'If (DS_data.Tables.Count > 0) Then

        '    If DS_data.Tables(0).Rows.Count > 0 Then

        '        Dim dtAccounts As DataTable = DS_data.Tables(0)

        '        txthrgangkutBJ_25.Text = "0.00"

        '        For Each row As DataRow In dtAccounts.Rows
        '            txthrgangkutBJ_10.Text = dtAccounts.Rows(0)("HrgAngkut").ToString
        '            txthrgangkutBH.Text = dtAccounts.Rows(1)("HrgAngkut").ToString
        '            If DC_HgAngkt.JenisStok = "BJ" Then
        '                txthrgangkutBH.Enabled = False
        '            Else
        '                txthrgangkutBJ_10.Enabled = False
        '                txthrgangkutBJ_25.Enabled = False
        '            End If
        '        Next



        '    End If

        '    txtkosbj10.Text = DC_Pdlmn.Kos_10kg
        '    txtkosobj25.Text = DC_Pdlmn.Kos_25kg
        '    txtkosbenih.Text = DC_Pdlmn.Kos_Benih

        'End If


        'End If
        'End If




    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click

        Dim GComp = New TSOnline.Business.GeneralComponent()
        Dim data_HgaAngktn As New DC_HrgAngkutan
        Dim data_Pedalaman As New DC_Pedalaman
        Dim jenisangkut As String = Trim(ddljnsangkut.SelectedValue)

        Try

            Using scope As New TransactionScope(TransactionScopeOption.Required)


                If (Page.IsValid) Then

                    If Request.QueryString("NoSiri_Hrg") = "" And Request.QueryString("Pedalaman_ID") = "" Then

                        If Trim(ddljnsangkut.SelectedValue) = "Kilang–Stor" Or Trim(ddljnsangkut.SelectedValue) = "SP-PT" Or Trim(ddljnsangkut.SelectedValue) = "PT-PT" Then

                            'insert DC_HrgAngkutan only
                            '---------------------------
                            data_HgaAngktn.TujuanAngkut = Trim(ddljnsangkut.SelectedValue)
                            data_HgaAngktn.Lokasi = Trim(ddllokasi.SelectedValue)

                            data_HgaAngktn.KodHantar = Trim(ddlphntar.SelectedValue)
                            data_HgaAngktn.KodTerima = Trim(ddlpnerima.SelectedValue)

                            data_HgaAngktn.HrgAngkut = txthrgangkutBJ_10.Text
                            data_HgaAngktn.Ukuran = "10"

                            data_HgaAngktn.JenisStok = "BJ"

                            data_HgaAngktn.Status = "BARU"
                            data_HgaAngktn.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            If txttkhefektif.Text = "" Then
                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Sila isi Tarikh Efektif');", True)
                                Exit Sub
                            End If

                            data_HgaAngktn.Peg_Kemaskini = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP
                            data_HgaAngktn.Tkh_Kemaskini = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            GComp.AddHgaAngkt(data_HgaAngktn)

                            'Insert Audit Trail
                            '------------------
                            AuditTrail.Modul = "Kawalan Stok"
                            AuditTrail.Role = "Add"
                            AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                            AuditTrail.No_Rujukan = data_HgaAngktn.KodHantar + "-" + data_HgaAngktn.KodTerima
                            AuditTrail.Aktiviti = "Input Harga Angkut Fasa 1"
                            AuditTrail.Tarikh = DateTime.Now
                            AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                            MDComp.InsertAuditTrail(AuditTrail)

                        ElseIf Trim(ddljnsangkut.SelectedValue) = "SPT-PK" Or Trim(ddljnsangkut.SelectedValue) = "Pedalaman" Then

                            'insert DC_HrgAngkutan  (off dulu tak perlu post DC_HrgAngkutan sebab tiada keperluan ) by EQA 12102020
                            '---------------------- 
                            'If ddllokasi.SelectedValue = "Semenanjung" Then
                            '    For i As Integer = 1 To 2
                            '        data_HgaAngktn.TujuanAngkut = ddljnsangkut.SelectedValue
                            '        data_HgaAngktn.Lokasi = ddllokasi.SelectedValue

                            '        data_HgaAngktn.KodHantar = Trim(ddlphntar.SelectedValue)
                            '        data_HgaAngktn.KodTerima = Trim(ddlpnerima.SelectedValue)

                            '        If i = 1 Then
                            '            data_HgaAngktn.JenisStok = "BJ"
                            '            data_HgaAngktn.HrgAngkut = txthrgangkutBJ_10.Text
                            '            data_HgaAngktn.Ukuran = "10"
                            '        Else
                            '            data_HgaAngktn.JenisStok = "BH"
                            '            data_HgaAngktn.HrgAngkut = txthrgangkutBH.Text
                            '            data_HgaAngktn.Ukuran = "POKOK"
                            '        End If


                            '        data_HgaAngktn.Status = "BARU"
                            '        data_HgaAngktn.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            '        If txttkhefektif.Text = "" Then
                            '            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Sila isi Tarikh Efektif');", True)
                            '            Exit Sub
                            '        End If

                            '        data_HgaAngktn.Peg_Kemaskini = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP
                            '        data_HgaAngktn.Tkh_Kemaskini = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            '        GComp.AddHgaAngkt(data_HgaAngktn)
                            '    Next

                            'Else

                            '    For i As Integer = 1 To 3
                            '        data_HgaAngktn.TujuanAngkut = ddljnsangkut.SelectedValue
                            '        data_HgaAngktn.Lokasi = ddllokasi.SelectedValue

                            '        data_HgaAngktn.KodHantar = Trim(ddlphntar.SelectedValue)
                            '        data_HgaAngktn.KodTerima = Trim(ddlpnerima.SelectedValue)

                            '        If i = 1 Then
                            '            data_HgaAngktn.JenisStok = "BJ"
                            '            data_HgaAngktn.HrgAngkut = txthrgangkutBJ_10.Text
                            '            data_HgaAngktn.Ukuran = "10"
                            '        ElseIf i = 2 Then
                            '            data_HgaAngktn.JenisStok = "BJ"
                            '            data_HgaAngktn.HrgAngkut = txthrgangkutBJ_25.Text
                            '            data_HgaAngktn.Ukuran = "25"
                            '        ElseIf i = 3 Then
                            '            data_HgaAngktn.JenisStok = "BH"
                            '            data_HgaAngktn.HrgAngkut = txthrgangkutBH.Text
                            '            data_HgaAngktn.Ukuran = "POKOK"
                            '        End If


                            '        data_HgaAngktn.Status = "BARU"
                            '        data_HgaAngktn.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            '        If txttkhefektif.Text = "" Then
                            '            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Sila isi Tarikh Efektif');", True)
                            '            Exit Sub
                            '        End If

                            '        data_HgaAngktn.Peg_Kemaskini = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP
                            '        data_HgaAngktn.Tkh_Kemaskini = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            '        GComp.AddHgaAngkt(data_HgaAngktn)
                            '    Next

                            'End If


                            'insert DC_Pedalaman  
                            '---------------------- 
                            If ddljnsangkut.SelectedValue = "Pedalaman" Then
                                Dim usg = New TSOnline.Business.GeneralComponent()

                                If cbKG.Checked = False Then


                                    Dim dsKG As DataSet = MDComp.getkglist(ddl_mukim.SelectedValue, ddl_daerah.SelectedValue, ddl_negeri.SelectedValue)

                                    If (dsKG.Tables.Count > 0) Then

                                        Dim dt_KgList As DataTable = dsKG.Tables(0)

                                        If dsKG.Tables(0).Rows.Count > 0 Then

                                            For Each row As DataRow In dt_KgList.Rows
                                                data_Pedalaman.TujuanAngkut = ddljnsangkut.SelectedValue
                                                data_Pedalaman.Lokasi = ddllokasi.SelectedValue
                                                data_Pedalaman.KodHantar = ddlphntar.SelectedValue
                                                data_Pedalaman.U_Negeri_ID = ddl_negeri.SelectedValue
                                                data_Pedalaman.Kod_Daerah = ddl_daerah.SelectedValue
                                                data_Pedalaman.Kod_Mukim = ddl_mukim.SelectedValue
                                                data_Pedalaman.Kod_Kampung = row.Item("Kod_Kampung")
                                                data_Pedalaman.Kos_10kg = txtkosbj10.Text
                                                data_Pedalaman.Kos_25kg = txtkosobj25.Text
                                                data_Pedalaman.Kos_Benih = txtkosbenih.Text
                                                data_Pedalaman.Tkh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                                                data_Pedalaman.Status = "BARU"

                                                If rdaktif.Checked = True Then
                                                    data_Pedalaman.StatusAktif = "AKTIF"
                                                ElseIf rdxaktif.Checked = True Then
                                                    data_Pedalaman.StatusAktif = "TIDAK"
                                                End If

                                                data_Pedalaman.Tkh_Input = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                                                data_Pedalaman.Peg_Input = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                                                GComp.AddPedalaman(data_Pedalaman)
                                            Next

                                        End If

                                    End If
                                Else
                                    data_Pedalaman.TujuanAngkut = ddljnsangkut.SelectedValue
                                    data_Pedalaman.Lokasi = ddllokasi.SelectedValue
                                    data_Pedalaman.KodHantar = ddlphntar.SelectedValue
                                    data_Pedalaman.U_Negeri_ID = ddl_negeri.SelectedValue
                                    data_Pedalaman.Kod_Daerah = ddl_daerah.SelectedValue
                                    data_Pedalaman.Kod_Mukim = ddl_mukim.SelectedValue
                                    data_Pedalaman.Kod_Kampung = ddl_kg.SelectedValue
                                    data_Pedalaman.Kos_10kg = txtkosbj10.Text
                                    data_Pedalaman.Kos_25kg = txtkosobj25.Text
                                    data_Pedalaman.Kos_Benih = txtkosbenih.Text
                                    data_Pedalaman.Tkh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                                    data_Pedalaman.Status = "BARU"
                                    data_Pedalaman.StatusAktif = "AKTIF"
                                    data_Pedalaman.Tkh_Input = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                                    data_Pedalaman.Peg_Input = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                                    GComp.AddPedalaman(data_Pedalaman)

                                    'Insert Audit Trail
                                    '------------------
                                    AuditTrail.Modul = "Kawalan Stok"
                                    AuditTrail.Role = "Add"
                                    AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                                    AuditTrail.No_Rujukan = data_Pedalaman.KodHantar + "-" + data_Pedalaman.Kod_Kampung
                                    AuditTrail.Aktiviti = "Input Harga Angkut Fasa 2 Pedalaman"
                                    AuditTrail.Tarikh = DateTime.Now
                                    AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                                    MDComp.InsertAuditTrail(AuditTrail)

                                End If


                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Maklumat Berjaya Disimpan.');window.location ='DaftarHrgAngkutList.aspx';", True)


                            Else

                                data_HgaAngktn.NoSiri_Hrg = txtnosirihrg.Text
                                data_HgaAngktn.TujuanAngkut = ddljnsangkut.SelectedValue
                                data_HgaAngktn.Lokasi = ddllokasi.SelectedValue

                                data_HgaAngktn.KodHantar = ddlphntar.SelectedValue
                                data_HgaAngktn.KodTerima = ddlpnerima.SelectedValue

                                'data_HgaAngktn.HrgAngkut = txthrgangkutBJ.Text
                                data_HgaAngktn.JenisStok = "BJ"

                                data_HgaAngktn.Status = "BARU"
                                data_HgaAngktn.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                                If txttkhefektif.Text = "" Then
                                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Sila isi Tarikh Efektif');", True)
                                    Exit Sub
                                End If

                                data_HgaAngktn.Peg_Kemaskini = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP
                                data_HgaAngktn.Tkh_Kemaskini = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                                GComp.UpdateHgaAngkt(data_HgaAngktn)

                                'Insert Audit Trail
                                '------------------
                                AuditTrail.Modul = "Kawalan Stok"
                                AuditTrail.Role = "Edit"
                                AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                                AuditTrail.No_Rujukan = data_HgaAngktn.KodHantar + "-" + data_HgaAngktn.KodTerima
                                AuditTrail.Aktiviti = "Kemaskini Harga Angkut Fasa 1"
                                AuditTrail.Tarikh = DateTime.Now
                                AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                                MDComp.InsertAuditTrail(AuditTrail)

                                If ddljnsangkut.SelectedValue = "Pedalaman" Then
                                    data_Pedalaman.TujuanAngkut = ddljnsangkut.SelectedValue
                                    data_Pedalaman.Lokasi = ddllokasi.SelectedValue
                                    data_Pedalaman.KodHantar = ddlphntar.SelectedValue
                                    data_Pedalaman.U_Negeri_ID = ddl_negeri.SelectedValue
                                    data_Pedalaman.Kod_Daerah = ddl_daerah.SelectedValue
                                    data_Pedalaman.Kod_Mukim = ddl_mukim.SelectedValue
                                    data_Pedalaman.Kod_Kampung = ddl_kg.SelectedValue
                                    data_Pedalaman.Kos_10kg = txtkosbj10.Text
                                    data_Pedalaman.Kos_25kg = txtkosobj25.Text
                                    data_Pedalaman.Kos_Benih = txtkosbenih.Text
                                    data_Pedalaman.Tkh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                                    data_Pedalaman.Tkh_Input = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                                    data_Pedalaman.Peg_Input = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                                    GComp.UpdatePedalaman(data_Pedalaman)

                                    'Insert Audit Trail
                                    '------------------
                                    AuditTrail.Modul = "Kawalan Stok"
                                    AuditTrail.Role = "Edit"
                                    AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                                    AuditTrail.No_Rujukan = data_Pedalaman.KodHantar + "-" + data_Pedalaman.Kod_Kampung
                                    AuditTrail.Aktiviti = "Kemaskini Harga Angkut Fasa 1 Pedalaman"
                                    AuditTrail.Tarikh = DateTime.Now
                                    AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                                    MDComp.InsertAuditTrail(AuditTrail)

                                End If


                            End If

                        End If


                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Maklumat Berjaya Disimpan.');window.location ='DaftarHrgAngkutList.aspx';", True)


                    Else

                        If Trim(ddljnsangkut.SelectedValue) = "Kilang–Stor" Or Trim(ddljnsangkut.SelectedValue) = "SP-PT" Or Trim(ddljnsangkut.SelectedValue) = "PT-PT" Then

                            'update DC_HrgAngkutan only
                            '---------------------------
                            data_HgaAngktn.NoSiri_Hrg = txtnosirihrg.Text
                            data_HgaAngktn.TujuanAngkut = Trim(ddljnsangkut.SelectedValue)
                            data_HgaAngktn.Lokasi = Trim(ddllokasi.SelectedValue)

                            data_HgaAngktn.KodHantar = Trim(ddlphntar.SelectedValue)
                            data_HgaAngktn.KodTerima = Trim(ddlpnerima.SelectedValue)

                            data_HgaAngktn.HrgAngkut = txthrgangkutBJ_10.Text
                            data_HgaAngktn.Ukuran = "10"

                            data_HgaAngktn.JenisStok = "BJ"

                            data_HgaAngktn.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            If txttkhefektif.Text = "" Then
                                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Sila isi Tarikh Efektif');", True)
                                Exit Sub
                            End If

                            data_HgaAngktn.Peg_Kemaskini = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP
                            data_HgaAngktn.Tkh_Kemaskini = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                            GComp.UpdateHgaAngkt(data_HgaAngktn)

                            'Insert Audit Trail
                            '------------------
                            AuditTrail.Modul = "Kawalan Stok"
                            AuditTrail.Role = "Edit"
                            AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                            AuditTrail.No_Rujukan = data_HgaAngktn.KodHantar + "-" + data_HgaAngktn.KodTerima
                            AuditTrail.Aktiviti = "Kemaskini Harga Angkut Fasa 1"
                            AuditTrail.Tarikh = DateTime.Now
                            AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                            MDComp.InsertAuditTrail(AuditTrail)

                        End If


                        If ddljnsangkut.SelectedValue = "Pedalaman" Then
                            data_Pedalaman.TujuanAngkut = ddljnsangkut.SelectedValue
                            data_Pedalaman.KodHantar = ddlphntar.SelectedValue
                            data_Pedalaman.U_Negeri_ID = ddl_negeri.SelectedValue
                            data_Pedalaman.Kod_Daerah = ddl_daerah.SelectedValue
                            data_Pedalaman.Kod_Mukim = ddl_mukim.SelectedValue
                            data_Pedalaman.Kod_Kampung = ddl_kg.SelectedValue
                            data_Pedalaman.Kos_10kg = txtkosbj10.Text
                            data_Pedalaman.Kos_25kg = txtkosobj25.Text
                            data_Pedalaman.Kos_Benih = txtkosbenih.Text
                            data_Pedalaman.Tkh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                            data_Pedalaman.Tkh_Input = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                            data_Pedalaman.Peg_Input = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                            GComp.UpdatePedalaman(data_Pedalaman)

                            'Insert Audit Trail
                            '------------------
                            AuditTrail.Modul = "Kawalan Stok"
                            AuditTrail.Role = "Edit"
                            AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                            AuditTrail.No_Rujukan = data_Pedalaman.KodHantar + "-" + data_Pedalaman.Kod_Kampung
                            AuditTrail.Aktiviti = "Kemaskini Harga Angkut Fasa 2 Pedalaman"
                            AuditTrail.Tarikh = DateTime.Now
                            AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                            MDComp.InsertAuditTrail(AuditTrail)

                        End If

                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Maklumat Berjaya Dikemaskini.');window.location ='DaftarHrgAngkutList.aspx';", True)


                    End If

                End If


                scope.Complete()

            End Using

        Catch ex As TransactionAbortedException
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "TransactionAbortedException Message: {0}", ex.Message, True)

        Catch ex As ApplicationException
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "ApplicationException Message: {0}", ex.Message, True)


        End Try


        'ElseIf Trim(ddljnsangkut.SelectedValue) = "SPT-PK" Or Trim(ddljnsangkut.SelectedValue) = "Pedalaman" Then

        '            Dim bilkoditem As Integer = 2

        '            For i = 1 To bilkoditem

        '                If Request.QueryString("NoSiri_Hrg") = "" Then

        '                    data_HgaAngktn.TujuanAngkut = ddljnsangkut.SelectedValue
        '                    data_HgaAngktn.Lokasi = ddllokasi.SelectedValue

        '                    data_HgaAngktn.KodHantar = Trim(ddlphntar.SelectedValue)
        '                    'data_HgaAngktn.KodTerima = Trim(ddlpnerima.SelectedValue)

        '                    If i = 1 Then
        '                        Dim jenisstok As String = "BJ"
        '                        data_HgaAngktn.JenisStok = jenisstok

        '                        'data_HgaAngktn.HrgAngkut = txthrgangkutBJ.Text
        '                    Else
        '                        Dim jenisstok As String = "BH"
        '                        data_HgaAngktn.JenisStok = jenisstok

        '                        data_HgaAngktn.HrgAngkut = txthrgangkutBH.Text
        '                    End If


        '                    data_HgaAngktn.Status = "BARU"
        '                    data_HgaAngktn.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

        '                    If txttkhefektif.Text = "" Then
        '                        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Sila isi Tarikh Efektif');", True)
        '                        Exit Sub
        '                    End If

        '                    data_HgaAngktn.Peg_Kemaskini = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP
        '                    data_HgaAngktn.Tkh_Kemaskini = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

        '                    GComp.AddHgaAngkt(data_HgaAngktn)

        '                    If ddljnsangkut.SelectedValue = "SPT-PK(Pedalaman)" Then
        '                        data_Pedalaman.TujuanAngkut = ddljnsangkut.SelectedValue
        '                        data_Pedalaman.KodHantar = ddlphntar.SelectedValue
        '                        data_Pedalaman.U_Negeri_ID = ddl_negeri.SelectedValue
        '                        data_Pedalaman.Kod_Daerah = ddl_daerah.SelectedValue
        '                        data_Pedalaman.Kod_Mukim = ddl_mukim.SelectedValue
        '                        data_Pedalaman.Kod_Kampung = ddl_kg.SelectedValue
        '                        data_Pedalaman.Kos_10kg = txtkosbj10.Text
        '                        data_Pedalaman.Kos_25kg = txtkosobj25.Text
        '                        data_Pedalaman.Kos_Benih = txtkosbenih.Text
        '                        data_Pedalaman.Tkh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        '                        data_Pedalaman.Tkh_Input = DateTime.ParseExact(txt_tkhkemaskini.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)
        '                        data_Pedalaman.Peg_Input = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

        '                        GComp.AddPedalaman(data_Pedalaman)

        '                    End If

        '                    ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Maklumat Berjaya Dikemaskini.');window.location ='DaftarHrgAngkutList.aspx';", True)

        '                Else


        '        End If

        '            Next



        '        End If




    End Sub

    Private Sub btnLulus_Click(sender As Object, e As EventArgs) Handles btnLulus.Click

        Try

            Using scope As New TransactionScope(TransactionScopeOption.Required)

                Dim data_HgaAngktn As New DC_HrgAngkutan
                Dim data_Pdlmn As New DC_Pedalaman

                data_Pdlmn.Pedalaman_ID = Request.QueryString("Pedalaman_ID")

                data_HgaAngktn.NoSiri_Hrg = txtnosirihrg.Text

                If data_HgaAngktn.NoSiri_Hrg <> "" Then
                    txtPeg_Lulus.Text = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.UserName
                    data_HgaAngktn.Peg_Lulus = txtPeg_Lulus.Text

                    txtTkh_Lulus.Text = DateTime.Now.ToString("dd-MM-yyyy")
                    data_HgaAngktn.Tkh_Lulus = DateTime.ParseExact(txtTkh_Lulus.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                    GComp.UpdateLlusHgaAngkt(data_HgaAngktn) 'updating for table harga angkutan

                Else
                    txtPeg_Lulus.Text = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.UserName
                    data_Pdlmn.Peg_Lulus = txtPeg_Lulus.Text

                    txtTkh_Lulus.Text = DateTime.Now.ToString("dd-MM-yyyy")
                    data_Pdlmn.Tkh_Lulus = DateTime.ParseExact(txtTkh_Lulus.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                    GComp.UpdateLlusHgaPdlmn(data_Pdlmn) 'updating for table harga pedalaman
                End If


                ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Maklumat Berjaya Diluluskan.');window.location ='DaftarHrgAngkutList.aspx';", True)

                'Insert Audit Trail
                '------------------
                AuditTrail.Modul = "Kawalan Stok"
                AuditTrail.Role = "Approve"
                AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                AuditTrail.No_Rujukan = data_HgaAngktn.NoSiri_Hrg
                AuditTrail.Aktiviti = "Lulus Harga Angkut"
                AuditTrail.Tarikh = DateTime.Now
                AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                MDComp.InsertAuditTrail(AuditTrail)

                scope.Complete()

            End Using

        Catch ex As TransactionAbortedException
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "TransactionAbortedException Message: {0}", ex.Message, True)

        Catch ex As ApplicationException
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "ApplicationException Message: {0}", ex.Message, True)

        End Try


    End Sub

    Private Sub ddljnsangkut_TextChanged(sender As Object, e As EventArgs) Handles ddljnsangkut.TextChanged

        ddllokasi.SelectedValue = ""
        ddlphntar.SelectedValue = ""
        ddlpnerima.SelectedValue = ""

        If ddljnsangkut.SelectedValue = "Kilang–Stor" Then

            ddlpnerima.Visible = True
            pnerima.Visible = True

            hgaagktbh.Visible = False
            txthrgangkutBH.Visible = False

        ElseIf ddljnsangkut.SelectedValue = "SP-PT" Then

            hgaagktbh.Visible = False
            txthrgangkutBH.Visible = False

        ElseIf ddljnsangkut.SelectedValue = "SPT-PK" Then

            hgaagktbh.Visible = True
            txthrgangkutBH.Visible = True

        ElseIf ddljnsangkut.SelectedValue = "Pedalaman" Then

            ddlpnerima.Enabled = True

            BindPTSem()

            If ddllokasi.SelectedValue = "Semenanjung" Then
                BindPTSem()
            ElseIf ddllokasi.SelectedValue = "Sabah" Then
                BindPTSabah()
            ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                BindPTSarawak()
            End If

            hgaagktbh.Visible = True
            txthrgangkutBH.Visible = True

        End If

    End Sub


    Private Sub ddllokasi_TextChanged(sender As Object, e As EventArgs) Handles ddllokasi.TextChanged


        If ddljnsangkut.SelectedValue = "Kilang–Stor" Then

            SetKilangSPUR()

        ElseIf ddljnsangkut.SelectedValue = "SP-PT" Then

            SETSPURPT()

        ElseIf ddljnsangkut.SelectedValue = "SPT-PK" Then

            SetSPT()

        ElseIf ddljnsangkut.SelectedValue = "PT-PT" Then

            SetPTPT()

        ElseIf ddljnsangkut.SelectedValue = "Pedalaman" Then

            Bindddl_negeri()

            ddlpnerima.Visible = False
            pnerima.Visible = False

            If ddllokasi.SelectedValue = "Semenanjung" Then
                BindPTSem()
            ElseIf ddllokasi.SelectedValue = "Sabah" Then
                BindPTSabah()
            ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                BindPTSarawak()
            End If

            GetDictStokItem()

        End If

    End Sub


    Private Sub SetKilangSPUR()

        ddlpnerima.Visible = True
        pnerima.Visible = True
        txthrgangkutBJ_25.Text = "0.00"


        If ddllokasi.SelectedValue = "Semenanjung" Then
            BindKilang()
            BindPenerimaSPUR_Sem()
        ElseIf ddllokasi.SelectedValue = "Sabah" Then
            BindKilang()
            BindPenerimaSbh()
        ElseIf ddllokasi.SelectedValue = "Sarawak" Then
            BindKilang()
            BindPenerimaSwk()
        End If


    End Sub

    Private Sub SETSPURPT()

        ddlpnerima.Visible = True
        pnerima.Visible = True
        txthrgangkutBJ_25.Text = "0.00"


        If ddllokasi.SelectedValue = "Semenanjung" Then
            BindSPUR_Sem()
            BindPenerimaSem()
        ElseIf ddllokasi.SelectedValue = "Sabah" Then
            BindPTSabah()
            BindPenerimaSbh()
        ElseIf ddllokasi.SelectedValue = "Sarawak" Then
            BindPTSarawak()
            BindPenerimaSwk()
        End If


    End Sub


    Private Sub SetSPT()

        GetDictStokItem()


        If ddllokasi.SelectedValue = "Semenanjung" Then
            BindPTSem()
            BindPenerimaSem()
        ElseIf ddllokasi.SelectedValue = "Sabah" Then
            BindPTSabah()
            BindPenerimaSbh()
        ElseIf ddllokasi.SelectedValue = "Sarawak" Then
            BindPTSarawak()
            BindPenerimaSwk()
        End If

        ddlpnerima.Visible = False
        pnerima.Visible = False

    End Sub


    Private Sub SetPTPT()

        GetDictStokItem()

        If ddllokasi.SelectedValue = "Semenanjung" Then
            BindPTSem()
            BindPenerimaSem()
        ElseIf ddllokasi.SelectedValue = "Sabah" Then
            BindPTSabah()
            BindPenerimaSbh()
        ElseIf ddllokasi.SelectedValue = "Sarawak" Then
            BindPTSarawak()
            BindPenerimaSwk()
        End If


    End Sub


    Private Sub BindKilang()  ' Bind senarai kilang utk 1malaysia

        ddlphntar.Items.Clear()

        Dim PKilangMast As List(Of PembekalKilang_Mast) = AGComp.getkilanglist

        Dim _PKilangMast As PembekalKilang_Mast
        For Each _PKilangMast In PKilangMast
            If (Not String.IsNullOrEmpty(_PKilangMast.ID_SyktKilang)) Then
                Dim item As New ListItem
                item.Value = Trim(_PKilangMast.ID_SyktKilang)
                item.Text = Trim(_PKilangMast.ID_SyktKilang) + " - " + Trim(_PKilangMast.Nma_SyktKilang)
                ddlphntar.Items.Add(item)
            End If
        Next
        ddlphntar.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindSPUR_Sem()  'Bind SPUR utk Sem shj

        ddlphntar.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getSPURSemlist()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + Trim(_DC_Stor.NamaStor)
                ddlphntar.Items.Add(item)
            End If
        Next
        ddlphntar.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindPTSem()  'Bind stor PT penerima utk sem shj

        ddlphntar.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getStorPTSem()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + _DC_Stor.NamaStor.ToString
                ddlphntar.Items.Add(item)
            End If
        Next
        ddlphntar.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindPTSabah()  'Bind phantar stor PT shj utk Sbh

        ddlphntar.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getPTSabah()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + _DC_Stor.NamaStor.ToString
                ddlphntar.Items.Add(item)
            End If
        Next
        ddlphntar.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindPTSarawak()  'Bind phantar stor PT shj utk Swk

        ddlphntar.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getPTSarawak()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + _DC_Stor.NamaStor.ToString
                ddlphntar.Items.Add(item)
            End If
        Next
        ddlphntar.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindPenerimaSPUR_Sem()  'Bind SPUR utk Sem shj

        ddlpnerima.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getSPURSemlist()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + Trim(_DC_Stor.NamaStor)
                ddlpnerima.Items.Add(item)
            End If
        Next
        ddlpnerima.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindPenerimaSem()  'Bind stor PT penerima utk sem shj

        ddlpnerima.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getStorPTSem()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + _DC_Stor.NamaStor.ToString
                ddlpnerima.Items.Add(item)
            End If
        Next
        ddlpnerima.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub


    Private Sub BindPenerimaSbh()  'Bind stor PT penerima utk sbh shj

        ddlpnerima.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getPTSabah()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + _DC_Stor.NamaStor.ToString
                ddlpnerima.Items.Add(item)
            End If
        Next
        ddlpnerima.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub

    Private Sub BindPenerimaSwk()  'Bind stor PT penerima utk swk shj

        ddlpnerima.Items.Clear()

        Dim DC_Stor As List(Of DC_Stor) = AGComp.getPTSarawak()
        Dim _DC_Stor As DC_Stor
        For Each _DC_Stor In DC_Stor
            If (Not String.IsNullOrEmpty(_DC_Stor.KodStor)) Then
                Dim item As New ListItem
                item.Value = Trim(_DC_Stor.KodStor)
                item.Text = Trim(_DC_Stor.KodStor) + " - " + _DC_Stor.NamaStor.ToString
                ddlpnerima.Items.Add(item)
            End If
        Next
        ddlpnerima.Items.Insert(0, New ListItem("--Pilih--", ""))

    End Sub



    Public Sub GetDictStokItem()

        Dim usg = New TSOnline.Business.GeneralComponent()

        Dim dsstokitem As DataSet = MDComp.getdetaildictstokitem()

        If (dsstokitem.Tables.Count = 3) Then

            If dsstokitem.Tables(0).Rows.Count > 0 Then

                Dim dtAccounts As DataTable = dsstokitem.Tables(0)
                Dim dtAccounts1 As DataTable = dsstokitem.Tables(1)
                Dim dtAccounts2 As DataTable = dsstokitem.Tables(2)

                For Each row As DataRow In dtAccounts.Rows

                    If ddllokasi.SelectedValue = "Semenanjung" Then
                        txthrgangkutBJ_10.Text = row.Item("KosKendali_SG")
                    ElseIf ddllokasi.SelectedValue = "Sabah" Then
                        txthrgangkutBJ_10.Text = row.Item("KosKendali_SB")
                    ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                        txthrgangkutBJ_10.Text = row.Item("KosKendali_SW")
                    End If


                    For Each rows As DataRow In dtAccounts1.Rows
                        If ddllokasi.SelectedValue = "Semenanjung" Then
                            txthrgangkutBJ_25.Text = "0.00"
                        ElseIf ddllokasi.SelectedValue = "Sabah" Then
                            txthrgangkutBJ_25.Text = rows.Item("KosKendali_SB")
                        ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                            txthrgangkutBJ_25.Text = rows.Item("KosKendali_SW")
                        End If
                    Next

                    For Each rowss As DataRow In dtAccounts2.Rows

                        If ddllokasi.SelectedValue = "Semenanjung" Then
                            txthrgangkutBH.Text = rowss.Item("KKSG_BH")
                        ElseIf ddllokasi.SelectedValue = "Sabah" Then
                            txthrgangkutBH.Text = rowss.Item("KKSB_BH")
                        ElseIf ddllokasi.SelectedValue = "Sarawak" Then
                            txthrgangkutBH.Text = rowss.Item("KKSW_BH")
                        End If

                    Next
                Next

            End If

            If ddllokasi.SelectedValue = "Semenanjung" Then
                txtkosobj25.Text = "0.00"
            End If
        End If


    End Sub

    Public Sub Bindddl_negeri()
        ddl_negeri.Items.Clear()

        If ddllokasi.SelectedValue = "Semenanjung" Then

            Dim SP_NegeriList As List(Of SP_Negeri) = MDComp.SelectListSP_Negeri_SEM()

            Dim SP_Negeri As SP_Negeri
            For Each SP_Negeri In SP_NegeriList
                If (Not String.IsNullOrEmpty(SP_Negeri.U_Negeri_ID)) Then
                    Dim item As New ListItem
                    item.Value = SP_Negeri.U_Negeri_ID
                    item.Text = SP_Negeri.Negeri
                    ddl_negeri.Items.Add(item)

                End If
            Next

            ddl_negeri.Items.Insert(0, New ListItem("Sila Pilih", ""))

        Else

            Dim SP_NegeriList As List(Of SP_Negeri) = MDComp.SelectListSP_Negeri_SS(ddllokasi.SelectedValue)

            Dim SP_Negeri As SP_Negeri
            For Each SP_Negeri In SP_NegeriList
                If (Not String.IsNullOrEmpty(SP_Negeri.U_Negeri_ID)) Then
                    Dim item As New ListItem
                    item.Value = SP_Negeri.U_Negeri_ID
                    item.Text = SP_Negeri.Negeri
                    ddl_negeri.Items.Add(item)

                End If
            Next

            ddl_negeri.Items.Insert(0, New ListItem("Sila Pilih", ""))

        End If

    End Sub



    Protected Sub ddl_negeri_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddl_daerah.Items.Clear()
        ddl_mukim.Items.Clear()
        ddl_kg.Items.Clear()

        Dim SP_DaerahList As List(Of SP_Daerah) = MDComp.SelectListSP_DaerahByU_Negeri_ID(ddl_negeri.SelectedValue)

        Dim SP_Daerah As SP_Daerah
        For Each SP_Daerah In SP_DaerahList
            If (Not String.IsNullOrEmpty(SP_Daerah.Kod_Daerah)) Then
                Dim item As New ListItem
                item.Value = SP_Daerah.Kod_Daerah
                item.Text = SP_Daerah.Daerah
                ddl_daerah.Items.Add(item)

            End If
        Next


        ddl_daerah.Items.Insert(0, New ListItem("Sila Pilih", ""))


    End Sub

    Protected Sub ddl_daerah_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddl_mukim.Items.Clear()
        ddl_kg.Items.Clear()

        Dim SP_MukimList As List(Of SP_Mukim) = MDComp.SelectListSP_MukimByU_Daerah_IDU_Negeri_ID(ddl_daerah.SelectedValue, ddl_negeri.SelectedValue)

        Dim SP_Mukim As SP_Mukim
        For Each SP_Mukim In SP_MukimList
            If (Not String.IsNullOrEmpty(SP_Mukim.Kod_Mukim)) Then
                Dim item As New ListItem
                item.Value = SP_Mukim.Kod_Mukim
                item.Text = SP_Mukim.Mukim
                ddl_mukim.Items.Add(item)

            End If
        Next

        ddl_mukim.Items.Insert(0, New ListItem("Sila Pilih", ""))
        ddl_kg.Items.Insert(0, New ListItem("Sila Pilih", ""))
    End Sub


    Protected Sub ddl_mukim_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddl_kg.Items.Clear()

        Dim SP_KampungList As List(Of SP_Kampung) = MDComp.SelectListSP_KampungByKod_MukimKod_DaerahU_Negeri_ID(ddl_mukim.SelectedValue, ddl_daerah.SelectedValue, ddl_negeri.SelectedValue)

        Dim SP_Kampung As SP_Kampung
        For Each SP_Kampung In SP_KampungList
            If (Not String.IsNullOrEmpty(SP_Kampung.Kod_Kampung)) Then
                Dim item As New ListItem
                item.Value = SP_Kampung.Kod_Kampung
                item.Text = SP_Kampung.Kampung
                ddl_kg.Items.Add(item)

            End If
        Next

        ddl_kg.Items.Insert(0, New ListItem("Sila Pilih", ""))
    End Sub

    Protected Sub cbKG_CheckedChanged(sender As Object, e As EventArgs)

        If ddl_kg.Enabled = True Then
            If cbKG.Checked = False Then
                ddl_kg.Enabled = False
            End If
        Else
            ddl_kg.Enabled = True
        End If

    End Sub

    Private Sub btnKemaskini_Click(sender As Object, e As EventArgs) Handles btnKemaskini.Click

        Dim GComp = New TSOnline.Business.GeneralComponent()
        Dim data_HgaAgktPinda As New DC_HrgAngkutan
        Dim data_HgaAgktPindaPdlmn As New DC_Pedalaman
        Dim data_HgaAngktn As New Pinda_HargaAngkut
        Dim data_Pedalaman As New Pinda_HargaAngkut


        If Trim(ddljnsangkut.SelectedValue) = "Kilang–Stor" Or Trim(ddljnsangkut.SelectedValue) = "SP-PT" Then

            'insert DC_HrgAngkutan only
            '---------------------------
            DC_HgAngkt = MDComp.getdetailhrgangkt(Request.QueryString("NoSiri_Hrg"))

            data_HgaAngktn.TujuanAngkut = Trim(DC_HgAngkt.TujuanAngkut)
            data_HgaAngktn.Lokasi = DC_HgAngkt.Lokasi

            data_HgaAngktn.KodHantar = DC_HgAngkt.KodHantar
            data_HgaAngktn.KodTerima = DC_HgAngkt.KodTerima

            data_HgaAngktn.HrgAngkut = DC_HgAngkt.HrgAngkut
            data_HgaAngktn.Ukuran = "10"

            data_HgaAngktn.JenisStok = "BJ"

            data_HgaAngktn.Tarikh_Efektif = DC_HgAngkt.Tarikh_Efektif

            data_HgaAngktn.Peg_Kemaskini = DC_HgAngkt.Peg_Kemaskini
            data_HgaAngktn.Tkh_Kemaskini = DC_HgAngkt.Tkh_Kemaskini

            GComp.AddHgaAngkt_Pinda(data_HgaAngktn)


            'update data di table hargaangkut
            '----------------------------------
            data_HgaAgktPinda.NoSiri_Hrg = txtnosirihrg.Text
            data_HgaAgktPinda.HrgAngkut = txthrgangkutBJ_10.Text
            data_HgaAgktPinda.Tarikh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

            GComp.UpdateHgaAngkt_Pinda(data_HgaAgktPinda)


        ElseIf Trim(ddljnsangkut.SelectedValue) = "SPT-PK" Or Trim(ddljnsangkut.SelectedValue) = "Pedalaman" Then


            'insert dc_pedalaman
            '--------------------
            If ddljnsangkut.SelectedValue = "Pedalaman" Then

                Dim usg = New TSOnline.Business.GeneralComponent()

                DC_Pdlmn = MDComp.getdetailhrgangkt_Pdlmn(Request.QueryString("Pedalaman_ID"))

                If DC_Pdlmn.Lokasi = "Semenanjung" Then
                    For i As Integer = 1 To 2
                        data_Pedalaman.TujuanAngkut = DC_Pdlmn.TujuanAngkut
                        data_Pedalaman.KodHantar = DC_Pdlmn.KodHantar
                        data_Pedalaman.U_Negeri_ID = DC_Pdlmn.Negeri
                        data_Pedalaman.Kod_Daerah = DC_Pdlmn.Daerah
                        data_Pedalaman.Kod_Mukim = DC_Pdlmn.Mukim
                        data_Pedalaman.Kod_Kampung = DC_Pdlmn.Kampung

                        If i = 1 Then
                            data_Pedalaman.JenisStok = "BJ"
                            data_Pedalaman.Ukuran = "10"
                            data_Pedalaman.HrgAngkut = DC_Pdlmn.Kos_10kg
                        ElseIf i = 2 Then
                            data_Pedalaman.JenisStok = "BH"
                            data_Pedalaman.Ukuran = "POKOK"
                            data_Pedalaman.HrgAngkut = DC_Pdlmn.Kos_Benih
                        End If

                        data_Pedalaman.Tarikh_Efektif = DC_Pdlmn.Tarikh_Efektif
                        data_Pedalaman.Tkh_Kemaskini = DC_Pdlmn.Tkh_Kemaskini
                        data_Pedalaman.Peg_Kemaskini = DC_Pdlmn.Peg_Kemaskini

                        GComp.AddHgaAngkt_Pinda(data_Pedalaman)

                    Next
                Else
                    For i As Integer = 1 To 3
                        data_Pedalaman.TujuanAngkut = DC_Pdlmn.TujuanAngkut
                        data_Pedalaman.KodHantar = DC_Pdlmn.KodHantar
                        data_Pedalaman.U_Negeri_ID = DC_Pdlmn.Negeri
                        data_Pedalaman.Kod_Daerah = DC_Pdlmn.Daerah
                        data_Pedalaman.Kod_Mukim = DC_Pdlmn.Mukim
                        data_Pedalaman.Kod_Kampung = DC_Pdlmn.Kampung

                        If i = 1 Then
                            data_Pedalaman.JenisStok = "BJ"
                            data_Pedalaman.Ukuran = "10"
                            data_Pedalaman.HrgAngkut = DC_Pdlmn.Kos_10kg
                        ElseIf i = 2 Then
                            data_Pedalaman.JenisStok = "BJ"
                            data_Pedalaman.Ukuran = "25"
                            data_Pedalaman.HrgAngkut = DC_Pdlmn.Kos_25kg
                        Else
                            data_Pedalaman.JenisStok = "BH"
                            data_Pedalaman.Ukuran = "POKOK"
                            data_Pedalaman.HrgAngkut = DC_Pdlmn.Kos_Benih
                        End If

                        data_Pedalaman.Tarikh_Efektif = DC_Pdlmn.Tarikh_Efektif
                        data_Pedalaman.Tkh_Kemaskini = DC_Pdlmn.Tkh_Kemaskini
                        data_Pedalaman.Peg_Kemaskini = DC_Pdlmn.Peg_Kemaskini

                        GComp.AddHgaAngkt_Pinda(data_Pedalaman)

                    Next
                End If


                'update data di table hargaangkut
                '----------------------------------
                If rdaktif.Checked = True Then
                    data_HgaAgktPindaPdlmn.StatusAktif = "AKTIF"
                ElseIf rdxaktif.Checked = True Then
                    data_HgaAgktPindaPdlmn.StatusAktif = "TIDAK"
                End If

                data_HgaAgktPindaPdlmn.Pedalaman_ID = DC_Pdlmn.Pedalaman_ID
                data_HgaAgktPindaPdlmn.Kos_10kg = txtkosbj10.Text
                data_HgaAgktPindaPdlmn.Kos_25kg = txtkosobj25.Text
                data_HgaAgktPindaPdlmn.Kos_Benih = txtkosbenih.Text
                data_HgaAgktPindaPdlmn.Tkh_Efektif = DateTime.ParseExact(txttkhefektif.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture)

                GComp.UpdateHgaAngkt_PindaPdlmn(data_HgaAgktPindaPdlmn)


                'Insert Audit Trail
                '------------------
                AuditTrail.Modul = "Kawalan Stok"
                AuditTrail.Role = "Modify"
                AuditTrail.URL = "/Stok/DaftarHrgAngkut"
                AuditTrail.No_Rujukan = data_HgaAgktPinda.NoSiri_Hrg
                AuditTrail.Aktiviti = "Kemaskini Harga Angkut"
                AuditTrail.Tarikh = DateTime.Now
                AuditTrail.User_Id = TSOnline.UI.Web.UIHelper.GetCurrentUserInfo.No_KP

                MDComp.InsertAuditTrail(AuditTrail)

            End If

        End If


        ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "alert", "alert('Pindaan Berjaya Disimpan.');window.location ='DaftarHrgAngkutList.aspx';", True)

    End Sub


End Class