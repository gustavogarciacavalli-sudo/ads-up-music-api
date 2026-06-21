import { useEffect, useState } from 'react';
import { Track } from '../types/Track';
import { Artist } from '../types/Artist';
import { trackApi, artistApi } from '../api/api';
import Loading from '../components/Loading';
import ErrorMessage from '../components/ErrorMessage';

export default function TracksPage() {
  const [tracks, setTracks] = useState<Track[]>([]);
  const [artists, setArtists] = useState<Artist[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [formError, setFormError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editId, setEditId] = useState<number | null>(null);
  const [form, setForm] = useState({ title: '', bpm: 0, duration: '', artistId: 0 });

  useEffect(() => { loadData(); }, []);

  async function loadData() {
    try {
      setLoading(true); setError('');
      const [t, a] = await Promise.all([trackApi.getAll(), artistApi.getAll()]);
      setTracks(t); setArtists(a);
    } catch { setError('Erro ao carregar músicas.'); }
    finally { setLoading(false); }
  }

  function openCreate() {
    setForm({ title: '', bpm: 120, duration: '3:30', artistId: artists.length > 0 ? artists[0].id : 0 });
    setEditId(null); setFormError(''); setShowForm(true);
  }

  function openEdit(track: Track) {
    setForm({ title: track.title, bpm: track.bpm, duration: track.duration, artistId: track.artistId });
    setEditId(track.id); setFormError(''); setShowForm(true);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault(); setFormError('');
    if (!form.title.trim()) { setFormError('Título é obrigatório.'); return; }
    if (form.bpm <= 0) { setFormError('BPM deve ser um valor positivo.'); return; }
    if (!form.artistId) { setFormError('Selecione um artista.'); return; }
    try {
      if (editId) { await trackApi.update(editId, form); }
      else { await trackApi.create(form); }
      setShowForm(false); loadData();
    } catch (err: any) { setFormError(err.message || 'Erro ao salvar música.'); }
  }

  async function handleDelete(id: number) {
    if (!confirm('Deseja realmente excluir esta música?')) return;
    try { await trackApi.delete(id); loadData(); }
    catch (err: any) { setError(err.message || 'Erro ao excluir música.'); }
  }

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">🎶 Músicas</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Nova Música</button>
      </div>

      {showForm && (
        <div className="modal-overlay" onClick={() => setShowForm(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{editId ? 'Editar Música' : 'Nova Música'}</h2>
            {formError && <div className="form-error">{formError}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Título *</label>
                <input type="text" value={form.title} onChange={e => setForm({...form, title: e.target.value})} placeholder="Título da música" />
              </div>
              <div className="form-row">
                <div className="form-group">
                  <label>BPM *</label>
                  <input type="number" value={form.bpm} onChange={e => setForm({...form, bpm: Number(e.target.value)})} min="1" placeholder="120" />
                </div>
                <div className="form-group">
                  <label>Duração</label>
                  <input type="text" value={form.duration} onChange={e => setForm({...form, duration: e.target.value})} placeholder="3:30" />
                </div>
              </div>
              <div className="form-group">
                <label>Artista *</label>
                <select value={form.artistId} onChange={e => setForm({...form, artistId: Number(e.target.value)})}>
                  <option value={0}>Selecione um artista</option>
                  {artists.map(a => <option key={a.id} value={a.id}>{a.name}</option>)}
                </select>
              </div>
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">{editId ? 'Salvar' : 'Criar'}</button>
                <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>Cancelar</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {tracks.length === 0 ? (
        <p className="empty-state">Nenhuma música cadastrada.</p>
      ) : (
        <div className="table-container">
          <table className="data-table">
            <thead>
              <tr><th>ID</th><th>Título</th><th>BPM</th><th>Duração</th><th>Artista</th><th>Ações</th></tr>
            </thead>
            <tbody>
              {tracks.map(t => (
                <tr key={t.id}>
                  <td>{t.id}</td>
                  <td>{t.title}</td>
                  <td><span className="bpm-badge">{t.bpm}</span></td>
                  <td>{t.duration}</td>
                  <td>{t.artistName}</td>
                  <td className="actions-cell">
                    <button className="btn btn-sm btn-edit" onClick={() => openEdit(t)}>✏️ Editar</button>
                    <button className="btn btn-sm btn-delete" onClick={() => handleDelete(t.id)}>🗑️ Excluir</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
