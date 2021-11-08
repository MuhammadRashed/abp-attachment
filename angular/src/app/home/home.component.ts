import { AuthService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  get hasLoggedIn(): boolean {
    return this.oAuthService.hasValidAccessToken();
  }

  constructor(private oAuthService: OAuthService, private authService: AuthService,
    private fb: FormBuilder
    ) {


    }


  ngOnInit(): void {
    this.buildForm();
  }

  login() {
    this.authService.navigateToLogin();
  }


  form: FormGroup;
  buildForm() {
    this.form = this.fb.group({
      nameat : ["8405e6ce-3808-c4c3-c983-3a001370742b" , []]
      //nameat : [undefined , []] // use id from line 44
    });
  }

  submitForm() {
    if (this.form.invalid) return;
    let nameat = this.form.get('nameat').value;
    console.log('Attachment id: ' , nameat);
  }

}
