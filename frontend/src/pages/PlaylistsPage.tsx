import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Playlist } from '../types/Playlist';
import { playlistApi } from '../api/api';
import Loading from '../components/Loading';
import ErrorMessage from '../components/ErrorMessage';

export default function PlaylistsPage() {
  const navigate = useNavigate();
  const [playlists, setPlaylists] = useState<Playlist[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [formError, setFormError] = useState('');
  const [showForm, setShowForm] = useState(false);
  const [editId, setEditId] = useState<number | null>(null);
  const [form, setForm] = useState({ name: '', description: '', mood: '' });

  useEffect(() => { loadPlaylists(); }, []);

  async function loadPlaylists() {
    try {
      setLoading(true); setError('');
      const data = await playlistApi.getAll();
      setPlaylists(data);
    } catch { setError('Erro ao carregar playlists.'); }
    finally { setLoading(false); }
  }

  function openCreate() {
    setForm({ name: '', description: '', mood: '' });
    setEditId(null); setFormError(''); setShowForm(true);
  }

  function openEdit(playlist: Playlist) {
    setForm({ name: playlist.name, description: playlist.description, mood: playlist.mood });
    setEditId(playlist.id); setFormError(''); setShowForm(true);
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault(); setFormError('');
    if (!form.name.trim()) { setFormError('Nome é obrigatório.'); return; }
    try {
      if (editId) { await playlistApi.update(editId, form); }
      else { await playlistApi.create(form); }
      setShowForm(false); loadPlaylists();
    } catch (err: any) { setFormError(err.message || 'Erro ao salvar playlist.'); }
  }

  async function handleDelete(id: number) {
    if (!confirm('Deseja realmente excluir esta playlist?')) return;
    try { await playlistApi.delete(id); loadPlaylists(); }
    catch (err: any) { setError(err.message || 'Erro ao excluir playlist.'); }
  }

  if (loading) return <Loading />;
  if (error) return <ErrorMessage message={error} />;

  return (
    <div className="page">
      <div className="page-header">
        <h1 className="page-title">📋 Playlists</h1>
        <button className="btn btn-primary" onClick={openCreate}>+ Nova Playlist</button>
      </div>

      {showForm && (
        <div className="modal-overlay" onClick={() => setShowForm(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>{editId ? 'Editar Playlist' : 'Nova Playlist'}</h2>
            {formError && <div className="form-error">{formError}</div>}
            <form onSubmit={handleSubmit}>
              <div className="form-group">
                <label>Nome *</label>
                <input type="text" value={form.name} onChange={e => setForm({...form, name: e.target.value})} placeholder="Nome da playlist" />
              </div>
              <div className="form-group">
                <label>Descrição</label>
                <textarea value={form.description} onChange={e => setForm({...form, description: e.target.value})} placeholder="Descrição" />
              </div>
              <div className="form-group">
                <label>Mood</label>
                <input type="text" value={form.mood} onChange={e => setForm({...form, mood: e.target.value})} placeholder="Ex: Animado, Relaxante, Focado" />
              </div>
              <div className="form-actions">
                <button type="submit" className="btn btn-primary">{editId ? 'Salvar' : 'Criar'}</button>
                <button type="button" className="btn btn-secondary" onClick={() => setShowForm(false)}>Cancelar</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {playlists.length === 0 ? (
        <p className="empty-state">Nenhuma playlist cadastrada.</p>
      ) : (
        <div className="cards-grid">
          {playlists.map(p => (
            <div key={p.id} className="playlist-card">
              <div className="playlist-card-header">
                <h3>{p.name}</h3>
                {p.mood && <span className="mood-badge">{p.mood}</span>}
              </div>
              <p className="playlist-card-desc">{p.description || 'Sem descrição'}</p>
              <p className="playlist-card-date">Criada em: {new Date(p.createdAt).toLocaleDateString('pt-BR')}</p>
              <div className="playlist-card-actions">
                <button className="btn btn-sm btn-primary" onClick={() => navigate(`/playlists/${p.id}`)}>📂 Detalhes</button>
                <button className="btn btn-sm btn-edit" onClick={() => openEdit(p)}>✏️ Editar</button>
                <button className="btn btn-sm btn-delete" onClick={() => handleDelete(p.id)}>🗑️ Excluir</button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
