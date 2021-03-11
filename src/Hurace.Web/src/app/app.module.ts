import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgxsModule } from '@ngxs/store';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { NgxsLoggerPluginModule } from '@ngxs/logger-plugin';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatInputModule, MatNativeDateModule } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatRadioModule } from '@angular/material/radio';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { SkierListComponent } from './components/skier-list/skier-list.component';
import { SkierEditComponent } from './components/skier-edit/skier-edit.component';
import { NothingSelectedComponent } from './components/shared/nothing-selected/nothing-selected.component';
import { RaceListComponent } from './components/race-list/race-list.component';
import { RaceDetailComponent } from './components/race-detail/race-detail.component';
import { CountryState } from './states/country.state';
import { SkierState } from './states/skier.state';
import { RaceState } from './states/race.state';
import { SeasonComponent } from './components/season/season.component';
import { SeasonState } from './states/season.state';
import { RaceStatisticComponent } from './components/race-statistic/race-statistic.component';
import { TabNavComponent } from './components/tab-nav/tab-nav.component';
import { LiveState } from './states/live.state';
import { LiveService } from './services/live.service';
import { SkierDetailComponent } from './components/skier-detail/skier-detail.component';
import { DataFieldComponent } from './components/shared/data-field/data-field.component';
import { SkierInfoComponent } from './components/skier-info/skier-info.component';
import { ErrorInfoComponent } from './components/shared/error-info/error-info.component';
import { LoadingComponent } from './components/shared/loading/loading.component';
import { IconInfoComponent } from './components/shared/icon-info/icon-info.component';
import { LiveStatisticComponent } from './components/live-statistic/live-statistic.component';
import { environment } from 'src/environments/environment';
import { RaceInfoComponent } from './components/shared/race-info/race-info.component';

export function initSignalR(liveService: LiveService) {
    return () => liveService.initSignalR();
}

@NgModule({
    declarations: [
        // Components
        AppComponent,
        SkierListComponent,
        SkierEditComponent,
        NothingSelectedComponent,
        RaceListComponent,
        RaceDetailComponent,
        SeasonComponent,
        RaceStatisticComponent,
        TabNavComponent,
        SkierDetailComponent,
        DataFieldComponent,
        SkierInfoComponent,
        ErrorInfoComponent,
        LoadingComponent,
        IconInfoComponent,
        LiveStatisticComponent,
        RaceInfoComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        BrowserAnimationsModule,
        FormsModule,
        ReactiveFormsModule,

        // Material Components
        MatButtonModule,
        MatListModule,
        MatIconModule,
        MatToolbarModule,
        MatTabsModule,
        MatInputModule,
        MatFormFieldModule,
        MatSelectModule,
        MatRadioModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatCheckboxModule,
        MatProgressSpinnerModule,
        MatTableModule,

        // Ngxs
        NgxsModule.forRoot([
            CountryState,
            SkierState,
            RaceState,
            SeasonState,
            LiveState
        ], { developmentMode: !environment.production }),
        NgxsReduxDevtoolsPluginModule.forRoot(),
        NgxsLoggerPluginModule.forRoot(),
    ],
    providers: [
        MatNativeDateModule,
        { provide: APP_INITIALIZER, useFactory: initSignalR, deps: [LiveService], multi: true }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
