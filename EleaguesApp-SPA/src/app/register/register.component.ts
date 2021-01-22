import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder
} from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { Router } from '@angular/router';
import { UserForRegister } from '../_models/UserForRegister';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: UserForRegister;
  file: File;
  countries = [
    { value: 'af', text: 'Afghanistan' },
    { value: 'au', text: 'Australia' },
    { value: 'bh', text: 'Bahrain' },
    { value: 'bb', text: 'Barbados' },
    { value: 'ca', text: 'Canada' },
    { value: 'de', text: 'Germany' },
    { value: 'in', text: 'India' },
    { value: 'it', text: 'Italy' },
    { value: 'pk', text: 'Pakistan' },
    { value: 'qa', text: 'Qatar' },
    { value: 'lc', text: 'Saint Lucia' },
    { value: 'sg', text: 'Singapore' },
    { value: 'se', text: 'Sweden' },
    { value: 'tt', text: 'Trinidad and Tobago' },
    { value: 'ae', text: 'UAE' },
    { value: 'gb', text: 'United Kingdom' },
    { value: 'us', text: 'USA' }
  ];

  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        username: ['', Validators.required],
        file: [''],
        knownAs: ['', Validators.required],
        psn: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        country: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        mobile: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(8)
          ]
        ],
        confirmPassword: ['', Validators.required]
      },
      { validator: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  onFileChanged(event) {
    this.file = event.target.files[0];
  }

  register() {
    if (this.registerForm.valid) {
      this.user = Object.assign({}, this.registerForm.value);

      if (this.file) {
        this.user.file = this.file;
      }

      this.authService.register(this.user).subscribe(
        () => {
          this.alertify.success('Registration succesful');
        },
        error => {
          this.alertify.error(error);
        },
        () => {
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          });
        }
      );
    }
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
