export interface Empty { kind: 'Empty' }
export interface Loading { kind: 'Loading' }
export interface Data<T> { kind: 'Data', data: T }
export interface Error { kind: 'Error', message: string }

export type ApiResource<T> = Empty | Loading | Data<T> | Error;

export const data = <T>(value: T): Data<T> => ({
    kind: 'Data', data: value
});

export const empty = (): Empty => ({ kind: 'Empty' });
export const loading = (): Loading => ({ kind: 'Loading' });
export const error = (message: string): Error => ({ kind: 'Error', message });
