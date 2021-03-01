import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SkierListComponent } from './components/skier-list/skier-list.component';
import { NothingSelectedComponent } from './components/shared/nothing-selected/nothing-selected.component';
import { RaceListComponent } from './components/race-list/race-list.component';
import { RaceDetailComponent } from './components/race-detail/race-detail.component';
import { SeasonComponent } from './components/season/season.component';
import { RaceStatisticComponent } from './components/race-statistic/race-statistic.component';
import { SkierDetailComponent } from './components/skier-detail/skier-detail.component';

const routes: Routes = [
    { path: '', redirectTo: 'skiers', pathMatch: 'full' },
    { path: 'skiers', component: SkierListComponent, children: [
        { path: ':id', component: SkierDetailComponent },
        { path: '**', component: NothingSelectedComponent }
    ] },
    { path: 'races', component: RaceListComponent, children: [
        { path: ':id', component: RaceDetailComponent, children: [
            { path: ':runNumber', component: RaceStatisticComponent },
            { path: '', redirectTo: '1', pathMatch: 'full' }
        ] },
        { path: '**', component: NothingSelectedComponent }
    ] },
    { path: 'season', component: SeasonComponent },
    { path: '**', redirectTo: 'skiers' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

 }
